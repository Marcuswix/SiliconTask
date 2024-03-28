window.onbeforeunload = function () {
    sessionStorage.setItem("scrollPosition", window.scrollY);
    console.log("spara");
};

window.onload = function () {
    var scrollPosition = sessionStorage.getItem("scrollPosition");
    console.log("s�tter - ute");
    if (scrollPosition !== null) {
        console.log("s�tter - inne");
        window.scrollTo({
            top: parseInt(scrollPosition),
            behavior: "instant",
        });

        sessionStorage.removeItem("scrollPosition");
    }
};

