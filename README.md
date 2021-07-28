# SSD-Assignment-1

A simple e-commerce store developed with the Razor Pages framework and view engine coded entirely in CSharp.

For usage:
- Get a stripe account at <https://www.stripe.com>
- Register a test api key, DO NOT REGISTER A DEPLOYED KEY
- Get your keys and paste them into the .env.template file
- Rename the file to just .env

For Google OAuth Client ID:
- Go to https://developers.google.com/identity/sign-in/web/sign-in
- Create OAuth Client
- Get Client ID and Client Secret
- Put keys into local secrets manager (this to be changed)

To configure SendGrid:
- Go to sendgrid
- Create an account
- Register sender: cesars.creatures@outlook.com
- Verify email
- Create API key
- Put SendGrid User and the API key into local secrets manager (this to be changed)
