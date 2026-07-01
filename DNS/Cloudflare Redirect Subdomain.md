## TLDR:
Create redirect rule from template "Redirect to a different domain". Edit the incoming request value and the redirect expression, save, and when it pops up, tell it to create a proxied `A` DNS entry pointed to 192.0.2.1
## Response from Perplexity

### Prompt
In Cloudflare, I'm creating a redirect rule to redirect tech.jtenos.com to a GitHub URL. It requires that I create a CNAME. What should the value be, since it's proxied and goes somewhere else, it doesn't matter, but is there a good standard?
### Answer
For a proxied Cloudflare redirect, the CNAME target can be basically any valid hostname, because Cloudflare answers at the edge and the origin is never used. A common, clean choice is your apex domain itself, such as `jtenos.com`, or a placeholder like `www.jtenos.com` if you already use it elsewhere.[](https://developers.cloudflare.com/fundamentals/manage-domains/redirect-domain/)

Cloudflare’s redirect guidance specifically says the alias domain just needs a proxied A or CNAME record that resolves DNS, and it even recommends `192.0.2.1` as the standard dummy A-record value for this purpose. If you prefer a CNAME instead of an A record, point it to a hostname you control and that already exists, but the exact target does not matter for a proxied redirect as long as the record is valid.

For your case, I’d use one of these:

- `tech.jtenos.com -> jtenos.com`
- `tech.jtenos.com -> www.jtenos.com`

If you want the most standard setup, use an **A** record to `192.0.2.1` and proxy it, because that is the clearest convention Cloudflare documents for redirect-only hostnames.[](https://developers.cloudflare.com/fundamentals/manage-domains/redirect-domain/)