//slight background movement everywhere
var positions = ["left","center","right"];
var selected = Math.floor(Math.random()*3);
document.body.style.backgroundPosition = positions[selected];

setInterval(function () {
    var a = Math.floor(Math.random()*100);
    var b = Math.floor(Math.random()*100);
    document.body.style.backgroundPosition = a+"% " + b+"%";
}, 5000)


//changing text on buttons on hover in index and list
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

//adjusting style if invalid inputs were provided in create or edit
const fields = document.querySelectorAll('.field-group span');
fields.forEach(field => {
    //console.log(field.textContent)
    if (field.textContent !== ""){
        field.style.fontSize = '0.5em';
        field.style.color = '#bb2d3b';
        field.style.margin = '0 10px -1.5em';
    }
})