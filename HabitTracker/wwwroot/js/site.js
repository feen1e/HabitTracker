var positions = ["left","center","right"];
var selected = Math.floor(Math.random()*3);
document.body.style.backgroundPosition = positions[selected];

setInterval(function () {
    var a = Math.floor(Math.random()*100);
    var b = Math.floor(Math.random()*100);
    document.body.style.backgroundPosition = a+"% " + b+"%";
}, 5000)


const buttons = document.querySelectorAll('.completion-status');
buttons.forEach(button => {
    var defaultText = button.value;
    
    button.addEventListener('mouseover', function() {
        if (button.value === "Habit inactive") {
            return;
        }
        if (button.value === "Completed") {
            button.value = "Mark as incomplete"
        } else {
            button.value = "Mark as completed"
        }
    });

    button.addEventListener('mouseout', function() {
        button.value = defaultText;
    });
});