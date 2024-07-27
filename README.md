# Details Analysis Game Combat Logs

## Description
This project helps World of Warcraft players analyze high-level activities (like raids) and communicate (like chatting, posts, communities, etc.) between players (and any other persons). The project has a Desktop application and Web site and consists of 2 parts:
1. Analysis of game combat logs (World of Warcraft);
2. Simple social network.

## Features
- **Combat logs - uploading**: upload high-level activities log file, parsing and see readable content for analysis (like damage done, deaths, when executed actions, dashboard, used bursts, etc.);
- **Combat logs - details**: see information about uploaded combat logs (your or other users)
- **Graphics**: graphics that showed the contribution of each player and timelines (can be selected a timelines duration and see actions);
- **Profile**: use profile to communicate with other users;
- **Social network**: create posts to show other users your mind, chat with other users, create a community to collaborate with other users with your interests, see other users and main information;
- **Identity server**: create new profiles or login (if you already have an account) using Identity Server. Identity Server allows security users to access private data and reduces risks of illegal access to your data.

## Installation
1. Clone the repository;
2. Change SQL DB Server name in the Connection string (Development configs);
3. Change API ports in App.config in Desktop app (if you will run Desktop application);
4. Build all projects;
5. (Optional) Run .Net tests and Web app tests ('npm test' in CombatAnalysis.WebApp/ClientApp directory);
6. Run all API, Identity Server, and Desktop or/and Web application(s);

### Project participants/Developers
1. Solinx97 (https://github.com/Solinx97)