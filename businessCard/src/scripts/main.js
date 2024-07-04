function sectionSwitch() {
    let navigationItems = document.querySelectorAll(".navigation__container .list");

    navigationItems.forEach((item, index) => {
        item.addEventListener("click", (e) => {
            let last = e.target.getAttribute("data-last");
            let data = e.target.getAttribute("data-content");
            let aboutMeItem = document.querySelector(`.about-me__${last}`);

            aboutMeItem.classList.add("passed");

            let aboutMeCollections = document.querySelector(".about-me");
            aboutMeCollections.children[index].classList.add("start");
            aboutMeCollections.children[index].classList.add("active");

            setTimeout(() => {
                aboutMeItem.classList.remove("active");
                aboutMeCollections.children[index].classList.remove("start");
            }, 1000);
        });
    });
}

sectionSwitch();

function appSwitch() {
    let appSwitcher = document.querySelector("#app-switcher");
    let desktopApp = document.querySelector(".applications__desktop-application");
    let webApp = document.querySelector(".applications__web-application");
    let desktopAppNavigation = document.querySelector(".desktop-app-navigation");
    let webProjectNavigation = document.querySelector(".web-app-navigation");

    appSwitcher.addEventListener("change", (e) => {
        if (appSwitcher.checked) {
            webApp.classList.remove("active");
            desktopApp.classList.add("active");

            desktopAppNavigation.classList.add("active");
            webProjectNavigation.classList.remove("active");
        }
        else {
            desktopApp.classList.remove("active");
            webApp.classList.add("active");

            desktopAppNavigation.classList.remove("active");
            webProjectNavigation.classList.add("active");
        }
    });
}

function projectSwitch() {
    let openPersonalProject = document.querySelector(".open-personal-project");
    let closePersonalProject = document.querySelector(".close-personal-project");
    let personalProject = document.querySelector(".personal-project");
    let desktopAppNavigation = document.querySelector(".desktop-app-navigation");

    openPersonalProject.addEventListener("click", () => {
        openPersonalProject.classList.remove("active");
        closePersonalProject.classList.add("active");

        personalProject.classList.add("active");
        desktopAppNavigation.classList.add("active");
    });

    closePersonalProject.addEventListener("click", () => {
        closePersonalProject.classList.remove("active");
        openPersonalProject.classList.add("active");

        personalProject.classList.remove("active");
        desktopAppNavigation.classList.remove("active");
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

function useNavigate() {
    let list = document.querySelectorAll('li.list');
    for (let index = 0; index < list.length; index++) {
        list[index].onmouseover = () => {
            let j = 0;
            while (j < list.length) {
                list[j++].className = 'list';
            }
            list[index].className = 'list active';
        }
    }
}

useNavigate();

function showSkillsDetails() {
    let list = document.querySelectorAll('.skill-level .name');
    for (let index = 0; index < list.length; index++) {
        list[index].onmouseover = () => {
            let j = 0;
            while (j < list.length) {
                list[j++].className = 'name';
            }
            list[index].className = 'name active';
        }
    }
}

showSkillsDetails();

function goToDesktopSection() {
    let goTo = document.querySelectorAll('.desktop-app-navigation .go-to .title');
    let sections = document.querySelectorAll('.applications__desktop-application .go-to-target');

    for (let index = 0; index < goTo.length; index++) {
        goTo[index].addEventListener("click", () => {
            clearActiveSection(sections);

            sections[index].classList.add("active");
        });
    }

    function clearActiveSection(sections) {
        for (let index = 0; index < sections.length; index++) {
            sections[index].classList.remove("active");
        }
    }
}

function goToWebSection() {
    let goTo = document.querySelectorAll('.web-app-navigation .go-to .title');
    let sections = document.querySelectorAll('.applications__web-application .go-to-target');

    for (let index = 0; index < goTo.length; index++) {
        goTo[index].addEventListener("click", () => {
            clearActiveSection(sections);

            sections[index].classList.add("active");
        });
    }

    function clearActiveSection(sections) {
        for (let index = 0; index < sections.length; index++) {
            sections[index].classList.remove("active");
        }
    }
}

goToDesktopSection();
goToWebSection();