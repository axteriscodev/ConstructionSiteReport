

export function navigaPagina(idElement) {
    console.log("id: " + idElement);
    let element = document.getElementById(idElement);
    console.log("elemento: " + element);
    if (element != null) {

        element.scrollIntoView();
    }
}