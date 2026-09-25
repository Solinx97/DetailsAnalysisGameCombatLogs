export const statusCode = {
    notAuthorized: 401
};

export const pageWithoutAuth = [
    "/", "/login", "/registration", 
    "/game-combat-logs","/general-analysis", "/general-analysis/auras", "/wow-game-data", "/wow-game-data/callback",
    "/general-analysis/watch", "/selected-combat", "/combat-details",
    "/callback", "/player-movements"
];

export const unautorizedRedirectTo = "/";