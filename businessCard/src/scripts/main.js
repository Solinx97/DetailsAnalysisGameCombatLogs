function appSwitch() {
    let appSwitcher = document.querySelector("#app-switcher");
    let desktopApp = document.querySelector(".applications__desktop-application");
    let webApp = document.querySelector(".applications__web-application");

    appSwitcher.addEventListener("change", (e) => {
        if (appSwitcher.checked) {
            webApp.classList.remove("active");
            desktopApp.classList.add("active");
        }
        else {
            desktopApp.classList.remove("active");
            webApp.classList.add("active");
        }
    });
}

function projectSwitch() {
    let openPersonalProject = document.querySelector(".open-personal-project");
    let closePersonalProject = document.querySelector(".close-personal-project");
    let personalProject = document.querySelector(".personal-project");

    openPersonalProject.addEventListener("click", () => {
        openPersonalProject.classList.remove("active");
        closePersonalProject.classList.add("active");

        personalProject.classList.add("active");
    });

    closePersonalProject.addEventListener("click", () => {
        closePersonalProject.classList.remove("active");
        openPersonalProject.classList.add("active");

        personalProject.classList.remove("active");
    });
}

function desktopAppDownload() {
    let openPersonalProject = document.querySelector(".desktop-app-download");

    openPersonalProject.addEventListener("click", () => {
        window.open("https://install.appcenter.ms/users/Aleh_Fiadosau-epam.com/apps/DetalsAnalysisGamesCombatLogs-1/releases/2");
    });
}

appSwitch();
projectSwitch();
desktopAppDownload();