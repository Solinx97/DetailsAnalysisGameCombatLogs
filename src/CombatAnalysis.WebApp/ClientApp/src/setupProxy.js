const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:62166';

const context = [
    "/api/v1/MainInformation",
    "/api/v1/Account",
    "/api/v1/Customer",
    "/api/v1/Friend",
    "/api/v1/Post",
    "/api/v1/PostComment",
    "/api/v1/PostLike",
    "/api/v1/PostDislike",
    "/api/v1/RequestToConnect",
    "/api/v1/BannedUser",
    "/api/v1/Authentication",
    "/api/v1/DamageDone",
    "/api/v1/DamageDoneGeneral",
    "/api/v1/DamageTaken",
    "/api/v1/DamageTakenGeneral",
    "/api/v1/DetailsSpecificalCombat",
    "/api/v1/GeneralAnalysis",
    "/api/v1/HealDone",
    "/api/v1/HealDoneGeneral",
    "/api/v1/ResourceRecovery",
    "/api/v1/ResourceRecoveryGeneral",
    "/api/v1/GroupChatMessage",
    "/api/v1/UnreadGroupChatMessage",
    "/api/v1/GroupChatUser",
    "/api/v1/GroupChat",
    "/api/v1/PersonalChat",
    "/api/v1/PersonalChatMessage",
    "/api/v1/PersonalChatMessageCount",
    "/api/v1/Community",
    "/api/v1/CommunityUser",
    "/api/v1/CommunityPost",
    "/api/v1/UserPost",
    "/api/v1/InviteToCommunity",
    "/api/v1/VoiceChat",
    "/api/v1/PlayerDeath",
    "/api/v1/Identity",
];

const onError = (err, req, resp, target) => {
    console.error(`${err.message}`);
}

module.exports = function (app) {
  const appProxy = createProxyMiddleware(context, {
    target: target,
    // Handle errors to prevent the proxy middleware from crashing when
    // the ASP NET Core webserver is unavailable
    onError: onError,
    secure: false,
    // Uncomment this line to add support for proxying websockets
    //ws: true, 
    headers: {
      Connection: 'Keep-Alive'
    }
  });

  app.use(appProxy);
};
