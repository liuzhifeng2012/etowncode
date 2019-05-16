var isie6 = window.XMLHttpRequest ? false : true;
window.onload = function () {
    try {
        var a = document.getElementById('cart');

        if (isie6) {
            a.style.position = 'absolute';
        } else {
            a.style.position = 'fixed';
        }
        a.style.right = '10';
        a.style.bottom = '10';
    } catch (e) {
        //alert(e.name + ": " + e.message);
    }
}