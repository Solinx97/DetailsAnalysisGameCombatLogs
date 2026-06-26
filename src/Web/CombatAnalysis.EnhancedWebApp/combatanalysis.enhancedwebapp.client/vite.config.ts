import { fileURLToPath, URL } from 'node:url';

import plugin from '@vitejs/plugin-react';
import fs from 'fs';
import { env } from 'process';
import { defineConfig, loadEnv } from 'vite';

const baseFolder =
    env.APPDATA !== undefined && env.APPDATA !== ''
        ? `${env.APPDATA}/ASP.NET/https`
        : `${env.HOME}/.aspnet/https`;

if (!fs.existsSync(baseFolder)) {
    fs.mkdirSync(baseFolder, { recursive: true });
}

const logsEndpoints = (target: string, apiVersion: string) => {
    return {
        [`^/api/${apiVersion}/Logs`]: { target, secure: false },
    }
}

const gameLogsEnpoints = (target: string, apiVersion: string) => {
    return {
        [`^/api/${apiVersion}/Boss`]: { target, secure: false },
        [`^/api/${apiVersion}/CombatAbility`]: { target, secure: false },
        [`^/api/${apiVersion}/PreAura`]: { target, secure: false },
        [`^/api/${apiVersion}/CombatLog`]: { target, secure: false },
        [`^/api/${apiVersion}/Combat`]: { target, secure: false },
        [`^/api/${apiVersion}/BossMap`]: { target, secure: false },
        [`^/api/${apiVersion}/CombatPlayer`]: { target, secure: false },
        [`^/api/${apiVersion}/CombatPlayerAura`]: { target, secure: false },
        [`^/api/${apiVersion}/CombatPlayerPosition`]: { target, secure: false },
        [`^/api/${apiVersion}/DamageDone`]: { target, secure: false },
        [`^/api/${apiVersion}/DamageDoneGeneral`]: { target, secure: false },
        [`^/api/${apiVersion}/DamageTaken`]: { target, secure: false },
        [`^/api/${apiVersion}/DamageTakenGeneral`]: { target, secure: false },
        [`^/api/${apiVersion}/HealDone`]: { target, secure: false },
        [`^/api/${apiVersion}/HealDoneGeneral`]: { target, secure: false },
        [`^/api/${apiVersion}/ResourceRecovery`]: { target, secure: false },
        [`^/api/${apiVersion}/ResourceRecoveryGeneral`]: { target, secure: false },
        [`^/api/${apiVersion}/PlayerDeath`]: { target, secure: false },
    }
}

const communityEndpoints = (target: string, apiVersion: string) => {
    return {
        [`^/api/${apiVersion}/Community`]: { target, secure: false },
        [`^/api/${apiVersion}/CommunityDiscussion`]: { target, secure: false },
        [`^/api/${apiVersion}/CommunityDiscussionComment`]: { target, secure: false },
        [`^/api/${apiVersion}/CommunityUser`]: { target, secure: false },
        [`^/api/${apiVersion}/InviteToCommunity`]: { target, secure: false },
    }
}

const userEndpoints = (target: string, apiVersion: string) => {
    return {
        [`^/api/${apiVersion}/User`]: { target, secure: false },
        [`^/api/${apiVersion}/Authentication`]: { target, secure: false },
        [`^/api/${apiVersion}/Customer`]: { target, secure: false },
        [`^/api/${apiVersion}/Friend`]: { target, secure: false },
        [`^/api/${apiVersion}/RequestToConnect`]: { target, secure: false },
        [`^/api/${apiVersion}/Identity`]: { target, secure: false },
    }
}

const feedEndpoints = (target: string, apiVersion: string) => {
    return {
        [`^/api/${apiVersion}/UserPost`]: { target, secure: false },
        [`^/api/${apiVersion}/UserPostLike`]: { target, secure: false },
        [`^/api/${apiVersion}/UserPostDislike`]: { target, secure: false },
        [`^/api/${apiVersion}/UserPostComment`]: { target, secure: false },
        [`^/api/${apiVersion}/CommunityPost`]: { target, secure: false },
        [`^/api/${apiVersion}/CommunityPostLike`]: { target, secure: false },
        [`^/api/${apiVersion}/CommunityPostDislike`]: { target, secure: false },
        [`^/api/${apiVersion}/CommunityPostComment`]: { target, secure: false },
    }
}

const chatEndpoints = (target: string, apiVersion: string) => {
    return {
        [`^/api/${apiVersion}/PersonalChat`]: { target, secure: false },
        [`^/api/${apiVersion}/PersonalChatMessage`]: { target, secure: false },
        [`^/api/${apiVersion}/GroupChat`]: { target, secure: false },
        [`^/api/${apiVersion}/GroupChatMessage`]: { target, secure: false },
        [`^/api/${apiVersion}/GroupChatUser`]: { target, secure: false },
        [`^/api/${apiVersion}/VoiceChat`]: { target, secure: false },
    }
}

const notificationEnddpoints = (target: string, apiVersion: string) => {
    return {
        [`^/api/${apiVersion}/Notification`]: { target, secure: false },
    }
}

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
    const env = loadEnv(mode, process.cwd(), '');

    const apiVersion = env.VITE_API_VERSION ? env.VITE_API_VERSION : 'v1';
    const target = env.VITE_APP_SERVER_PROXY_URL || 'http://localhost:5000';

    return {
        plugins: [plugin()],
        resolve: {
            alias: {
                '@': fileURLToPath(new URL('./src', import.meta.url))
            }
        },
        server: {
            proxy: {
                ...logsEndpoints(target, apiVersion),
                ...gameLogsEnpoints(target, apiVersion),
                ...communityEndpoints(target, apiVersion),
                ...userEndpoints(target, apiVersion),
                ...feedEndpoints(target, apiVersion),
                ...chatEndpoints(target, apiVersion),
                ...notificationEnddpoints(target, apiVersion),
            },
            port: parseInt(env.VITE_DEV_SERVER_PORT || '5173', 10)
        }
    }
})
