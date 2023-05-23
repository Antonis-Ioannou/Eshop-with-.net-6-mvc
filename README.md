# Eshop-with-.net-6-mvc

An online-shop to purchase books.
To run 
1rst replace the connection string in appsettings.json with your own 
2nd delete migrations, then enable-migrations and update database

For a succesfull payment, you'll need to get your own keys from Stripe, 
and paste them on the appsettings.json file ass well as type your local 
domain address on Areas => Customer => Controllers => CartController => line 135

If you want the email verification you'll need to generate your own 
keys from your email provider and add them in 
CirceBookUtility => EmailSender => line 35

For a successful facebook login, you'll need to generate your api keys and 
add them in CirceBookWeb => Program.cs => line 28 and line 29
