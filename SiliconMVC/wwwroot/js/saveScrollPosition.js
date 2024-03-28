window.onbeforeunload = function () {
    sessionStorage.setItem("scrollPosition", window.scrollY);
    console.log("spara");
};

window.onload = function () {
    var scrollPosition = sessionStorage.getItem("scrollPosition");
    console.log("sätter - ute");
    if (scrollPosition !== null) {
        console.log("sätter - inne");
        window.scrollTo({
            top: parseInt(scrollPosition),
            behavior: "instant",
        });

        sessionStorage.removeItem("scrollPosition");
    }
};

