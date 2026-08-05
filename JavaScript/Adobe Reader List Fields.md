```javascript
// Place in C:\Program Files (x86)\Adobe\Reader 11.0\Reader\Javascripts

    function listFormFields()
    {
       var allFields = "";

       for (var i = 0; i < this.numFields; i++)
       {
        if (i != 0 && i % 50 == 0) {
        app.alert(allFields);
        allFields = "";
        }
          var newField = '"' + this.getNthFieldName(i) + '" ';
          allFields += newField;
       }
       app.alert(allFields);
    }

    app.addMenuItem({
       cName: "JSListFormFields",
       cUser: "List all Form Fields",
       cParent: "File",
       cExec: "listFormFields()",
       cEnable: "event.rc = (event.target != null);",
       nPos: 0,
    });
    // end of script
```