In Windows, the OS seems to do a pretty good job storing your credentials for Git repos like Github or Bitbucket. In Linux, I had to type the password with every push.

But there’s a simple way to store the password with your local filesystem, so you don’t have to type it every time. Note that this stores your password in plain text, so don’t do this on a shared machine or anywhere you don’t feel comfortable storing plaintext passwords.

```bash
$ git config credential.helper store 
$ git push 
Username: <type your username> 
Password: <type your password>
```

Once the push is completed, you’ll see a new file in your home directory named .git-credentials, which contains your password and allows you to push without a password going forward.
