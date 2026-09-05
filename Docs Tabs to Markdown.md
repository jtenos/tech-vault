// Built by AI, no promises, but it seems to work ok

```javascript
/**
 * Google Docs → Markdown Exporter (Updated & Corrected)
 */
/**
 * Main entry point: Batch processes all Google Docs in a folder.
 */
function exportAllDocsInFolder() {
  const FOLDER_ID = 'xxxxxxxxxxxxxxxxxxxxxxx'; // Replace with your folder ID
  const folder = DriveApp.getFolderById(FOLDER_ID);
  const files = folder.getFilesByType(MimeType.GOOGLE_DOCS);

  let totalDocs = 0;

  while (files.hasNext()) {
    const file = files.next();
    const doc = DocumentApp.openById(file.getId());
    
    exportSingleDoc_(doc);
    totalDocs++;
    Logger.log(`Exported: ${doc.getName()}`);
  }

  Logger.log(`Finished processing ${totalDocs} document(s).`);
}

/**
 * Worker function for a single document.
 */
function exportSingleDoc_(doc) {
  const outputFolder = getOutputFolder_(doc);
  const imageFolder = getOrCreateFolder_(outputFolder, 'images');
  clearFolder_(imageFolder);

  const tabs = getAllDocumentTabs_(doc.getTabs());
  if (!tabs || tabs.length === 0) return;

  const context = { imageFolder: imageFolder, imageNumber: 0 };

  for (const tab of tabs) {
    const documentTab = tab.asDocumentTab();
    const tabName = tab.getTitle() || 'Untitled Tab';
    Logger.log(`Working on: ${tabName}`);
    const markdown = bodyToMarkdown_(documentTab.getBody(), context);
    const filename = sanitizeFilename_(tabName) + '.md';

    replaceFile_(outputFolder, filename, markdown);
  }

  if (context.imageNumber === 0) {
    imageFolder.setTrashed(true);
  }
}

/**
 * Recursively fetch top-level and nested sub-tabs.
 */
function getAllDocumentTabs_(tabs) {
  let list = [];
  if (!tabs) return list;

  for (const tab of tabs) {
    if (tab.getType() === DocumentApp.TabType.DOCUMENT_TAB) {
      list.push(tab);
    }
    const children = tab.getChildTabs();
    if (children && children.length > 0) {
      list = list.concat(getAllDocumentTabs_(children));
    }
  }

  return list;
}

/**
 * Convert a document body to Markdown.
 */
function bodyToMarkdown_(body, context) {
  const output = [];

  for (let i = 0; i < body.getNumChildren(); i++) {
    const element = body.getChild(i);

    const markdown = elementToMarkdown_(element, context);

    if (markdown !== '') {
      output.push(markdown);
    }
  }

  return cleanupMarkdown_(output.join('\n\n'));
}

/**
 * Convert a Google Docs element.
 */
function elementToMarkdown_(element, context) {
  switch (element.getType()) {

    case DocumentApp.ElementType.PARAGRAPH:
      return paragraphToMarkdown_(
        element.asParagraph(),
        context
      );

    case DocumentApp.ElementType.LIST_ITEM:
      return listItemToMarkdown_(
        element.asListItem(),
        context
      );

    case DocumentApp.ElementType.TABLE:
      return tableToMarkdown_(
        element.asTable(),
        context
      );

    case DocumentApp.ElementType.HORIZONTAL_RULE:
      return '---';

    default:
      return '';
  }
}

/**
 * Convert a paragraph, including floating/positioned images.
 */
function paragraphToMarkdown_(paragraph, context) {
  let content = inlineElementsToMarkdown_(paragraph, context);

  // Process positioned (wrap text / break text) images
  const positionedImages = paragraph.getPositionedImages();
  const posImagesMd = [];
  for (let i = 0; i < positionedImages.length; i++) {
    posImagesMd.push(imageToMarkdown_(positionedImages[i], context));
  }

  if (posImagesMd.length > 0) {
    const imagesBlock = posImagesMd.join('\n\n');
    content = content.trim() ? imagesBlock + '\n\n' + content : imagesBlock;
  }

  if (!content.trim()) {
    return '';
  }

  let prefix = '';

  switch (paragraph.getHeading()) {

    case DocumentApp.ParagraphHeading.TITLE:
    case DocumentApp.ParagraphHeading.HEADING1:
      prefix = '# ';
      break;

    case DocumentApp.ParagraphHeading.SUBTITLE:
    case DocumentApp.ParagraphHeading.HEADING2:
      prefix = '## ';
      break;

    case DocumentApp.ParagraphHeading.HEADING3:
      prefix = '### ';
      break;

    case DocumentApp.ParagraphHeading.HEADING4:
      prefix = '#### ';
      break;

    case DocumentApp.ParagraphHeading.HEADING5:
      prefix = '##### ';
      break;

    case DocumentApp.ParagraphHeading.HEADING6:
      prefix = '###### ';
      break;
  }

  return prefix + content;
}

/**
 * Convert a list item.
 */
function listItemToMarkdown_(item, context) {
  const content = inlineElementsToMarkdown_(item, context);

  if (!content.trim()) {
    return '';
  }

  const nestingLevel = item.getNestingLevel();
  const indent = '  '.repeat(nestingLevel);

  let marker;

  switch (item.getGlyphType()) {

    case DocumentApp.GlyphType.BULLET:
    case DocumentApp.GlyphType.HOLLOW_BULLET:
    case DocumentApp.GlyphType.SQUARE_BULLET:
      marker = '- ';
      break;

    default:
      marker = '1. ';
      break;
  }

  return indent + marker + content;
}

/**
 * Convert inline contents (Text, Inline Images).
 */
function inlineElementsToMarkdown_(container, context) {
  const output = [];

  for (let i = 0; i < container.getNumChildren(); i++) {
    const child = container.getChild(i);

    switch (child.getType()) {

      case DocumentApp.ElementType.TEXT:
        output.push(textToMarkdown_(child.asText()));
        break;

      case DocumentApp.ElementType.INLINE_IMAGE:
        output.push(imageToMarkdown_(child.asInlineImage(), context));
        break;

      default:
        break;
    }
  }

  return output.join('');
}

/**
 * Convert rich text to Markdown with CommonMark whitespace handling and code font detection.
 */
function textToMarkdown_(text) {
  const raw = text.getText();

  if (!raw) {
    return '';
  }

  const indices = text.getTextAttributeIndices();
  const pieces = [];

  for (let i = 0; i < indices.length; i++) {
    const start = indices[i];
    const end = (i + 1 < indices.length) ? indices[i + 1] : raw.length;

    let value = raw.substring(start, end);

    if (!value) {
      continue;
    }

    value = escapeMarkdownText_(value);

    const bold = text.isBold(start);
    const italic = text.isItalic(start);
    const strike = text.isStrikethrough(start);
    const link = text.getLinkUrl(start);
    const font = text.getFontFamily(start);

    // Monospace font detection for inline code blocks
    const isMonospace = font && (
      font.includes('Consolas') ||
      font.includes('Courier') ||
      font.includes('Source Code') ||
      font.includes('Monaco')
    );

    if (isMonospace) {
      value = wrapMarkdown_(value, '`', '`');
    } else {
      if (bold && italic) {
        value = wrapMarkdown_(value, '***', '***');
      } else if (bold) {
        value = wrapMarkdown_(value, '**', '**');
      } else if (italic) {
        value = wrapMarkdown_(value, '*', '*');
      }

      if (strike) {
        value = wrapMarkdown_(value, '~~', '~~');
      }
    }

    if (link) {
      value = '[' + value + '](' + escapeMarkdownUrl_(link) + ')';
    }

    pieces.push(value);
  }

  return pieces.join('');
}

/**
 * Safely places Markdown syntax markers around trimmed content to adhere to CommonMark.
 */
function wrapMarkdown_(text, prefix, suffix) {
  const leadingMatch = text.match(/^\s*/);
  const trailingMatch = text.match(/\s*$/);
  const leading = leadingMatch ? leadingMatch[0] : '';
  const trailing = trailingMatch ? trailingMatch[0] : '';
  const trimmed = text.trim();

  if (!trimmed) {
    return text;
  }

  return leading + prefix + trimmed + suffix + trailing;
}

/**
 * Extract an inline or positioned image and return its Markdown reference.
 */
function imageToMarkdown_(image, context) {
  context.imageNumber++;

  const blob = image.getBlob();

  let extension = getFileExtensionFromMimeType_(blob.getContentType());
  if (!extension) {
    extension = 'png';
  }

  const filename = 'image-' + String(context.imageNumber).padStart(3, '0') + '.' + extension;
  blob.setName(filename);

  context.imageFolder.createFile(blob);

  return '![' + filename + '](images/' + filename + ')';
}

/**
 * Convert a table to Markdown.
 */
function tableToMarkdown_(table, context) {
  const rowCount = table.getNumRows();

  if (rowCount === 0) {
    return '';
  }

  const output = [];

  const headerRow = table.getRow(0);
  const headerCells = [];

  for (let column = 0; column < headerRow.getNumCells(); column++) {
    headerCells.push(
      tableCellToMarkdown_(headerRow.getCell(column), context)
    );
  }

  output.push('| ' + headerCells.join(' | ') + ' |');
  output.push('| ' + headerCells.map(() => '---').join(' | ') + ' |');

  for (let row = 1; row < rowCount; row++) {
    const tableRow = table.getRow(row);
    const cells = [];

    for (let column = 0; column < tableRow.getNumCells(); column++) {
      cells.push(
        tableCellToMarkdown_(tableRow.getCell(column), context)
      );
    }

    output.push('| ' + cells.join(' | ') + ' |');
  }

  return output.join('\n');
}

/**
 * Convert a table cell.
 */
function tableCellToMarkdown_(cell, context) {
  const output = [];

  for (let i = 0; i < cell.getNumChildren(); i++) {
    const child = cell.getChild(i);

    switch (child.getType()) {

      case DocumentApp.ElementType.PARAGRAPH:
        output.push(inlineElementsToMarkdown_(child.asParagraph(), context));
        break;

      case DocumentApp.ElementType.LIST_ITEM:
        output.push(inlineElementsToMarkdown_(child.asListItem(), context));
        break;

      default:
        break;
    }
  }

  return output
    .join('<br>')
    .replace(/\|/g, '\\|')
    .replace(/\r?\n/g, '<br>')
    .trim();
}

/**
 * Escape Markdown text characters.
 */
function escapeMarkdownText_(text) {
  return text
    .replace(/\\/g, '\\\\')
    .replace(/`/g, '\\`')
    .replace(/\[/g, '\\[')
    .replace(/\]/g, '\\]')
    .replace(/^#/gm, '\\#')
    .replace(/^>/gm, '\\>');
}

/**
 * Escape URLs for Markdown syntax.
 */
function escapeMarkdownUrl_(url) {
  return url
    .replace(/\\/g, '\\\\')
    .replace(/\)/g, '\\)');
}

/**
 * Determine a sensible image extension.
 */
function getFileExtensionFromMimeType_(mimeType) {
  switch (mimeType) {
    case 'image/png': return 'png';
    case 'image/jpeg': return 'jpg';
    case 'image/gif': return 'gif';
    case 'image/webp': return 'webp';
    case 'image/svg+xml': return 'svg';
    case 'image/bmp': return 'bmp';
    default: return null;
  }
}

/**
 * Get/create output folder, ignoring trashed Google Drive items.
 */
function getOutputFolder_(doc) {
  const file = DriveApp.getFileById(doc.getId());
  const parents = file.getParents();

  let parentFolder = parents.hasNext() ? parents.next() : DriveApp.getRootFolder();
  const folderName = doc.getName();

  const existing = parentFolder.getFoldersByName(folderName);

  while (existing.hasNext()) {
    const folder = existing.next();
    if (!folder.isTrashed()) {
      return folder;
    }
  }

  return parentFolder.createFolder(folderName);
}

/**
 * Get or create a child folder, ignoring trashed items.
 */
function getOrCreateFolder_(parent, name) {
  const folders = parent.getFoldersByName(name);

  while (folders.hasNext()) {
    const folder = folders.next();
    if (!folder.isTrashed()) {
      return folder;
    }
  }

  return parent.createFolder(name);
}

/**
 * Clear folder contents (only non-trashed files).
 */
function clearFolder_(folder) {
  const files = folder.getFiles();
  while (files.hasNext()) {
    const f = files.next();
    if (!f.isTrashed()) f.setTrashed(true);
  }

  const folders = folder.getFolders();
  while (folders.hasNext()) {
    const f = folders.next();
    if (!f.isTrashed()) f.setTrashed(true);
  }
}

/**
 * Replace existing files safely by skipping already-trashed items.
 */
function replaceFile_(folder, filename, contents) {
  const existing = folder.getFilesByName(filename);

  while (existing.hasNext()) {
    const f = existing.next();
    if (!f.isTrashed()) {
      f.setTrashed(true);
    }
  }

  folder.createFile(filename, contents, MimeType.PLAIN_TEXT);
}

/**
 * Make a safe filename from a tab name.
 */
function sanitizeFilename_(name) {
  return name
    .replace(/[\\\/:*?"<>|#%{}]/g, '_')
    .replace(/\s+/g, ' ')
    .trim()
    .replace(/\.+$/, '')
    || 'Untitled Tab';
}

/**
 * Normalize Markdown spacing output.
 */
function cleanupMarkdown_(markdown) {
  return markdown
    .replace(/\n{3,}/g, '\n\n')
    .trim() + '\n';
}
```