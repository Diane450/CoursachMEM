//маска ввода номера телефона
document.getElementById('phone').addEventListener('input', function (e) {
    var x = e.target.value.replace(/\D/g, '').match(/(\d{0,1})(\d{0,3})(\d{0,3})(\d{0,2})(\d{0,2})/);
    e.target.value = '8 ' + (!x[2] ? '' : '(' + x[2] + (!x[3] ? '' : ')' + ' ' + x[3] + (!x[4] ? '' : '-' + x[4] + (!x[5] ? '' : '-' + x[5]))));
});
//показать/спрятать пароль
function show_hide_password() {
    var input = document.getElementById('newUserPassword');
    if (input.type === 'password') {
    input.type = 'text';
    }
    else {
    input.type = 'password';
    }
}
//установить минимальную датуSetMinDate

function SetMinDate() {

    let today = new Date();
    let day = today.getDate(),
        month = today.getMonth() + 1,
        year = today.getFullYear();

    if (month < 10) month = "0" + month;
    if (day < 10) day = "0" + day;

    const min = `${year}-${month}-${day}`;

    document.getElementById("date").defaultValue = min;
    document.getElementById("date").setAttribute("min", min);
}