# Basic Chat API

This is a cool little chat API I made.

## Features

### Accountless
You don't need an account to get and send messages using the API, it works somewhat like TeamSpeak. 
This does come with the con of multiple people with the name being able to join.

### Privacy
The server knows pretty much nothing about you, only your IP if debug logging is enabled.

### User Verification
You can verify the sender of a message via a signature and public key that clients can choose to send.

### Optional Online Indicator
Clients can get a list of online users, although clients can opt out of appearing online by simply not sending the username value in the request.

### FOSS
You can't really beat FOSS can you.
