
/**
 * Metodo usato per creare il PDF del documento compilato
 * @param {any} filename il nome del file
 * @returns il file pdf come download dal browser
 */
export async function generaPDFDocumento(filename, dotnet) {

    const doc = new jspdf.jsPDF({
        orientation: 'p',
        unit: 'pt',
        format: 'a4',

    });

    const content = document.getElementById('printPDF').innerHTML;
    console.log("content: " + content.offsetHeight);
    //seleziono gli elementi che 
    const elements = document.querySelectorAll('.pdfElement');
    let yPosition = 15;
    let xPosition = 12;
    let pageCount = 0;
    let currentSize = 0;
    let pageHeight = doc.internal.pageSize.getHeight();
    let pages = [];
    console.log("ph= " + pageHeight);

    let tempDiv = document.createElement('div');
    pages.push(tempDiv);
    //Ciclo ogni elemento per verificare se aggiungerlo in una pagina o creare una pagina nuova in base all'altezza
    elements.forEach((elem, index) => {
        console.log(elem.innerHTML);
        let altezza = elem.offsetHeight + currentSize;
        //console.log(" currentSize= " + currentSize + " offsetHeight= " + elem.offsetHeight);
        //console.log(" pagina " + pageCount + " altezza= " + altezza);
        if (altezza < 1350) {
            //console.log("dentro if, pagina " + pageCount);
            pages[pageCount].innerHTML += elem.innerHTML;
            currentSize = altezza;
        } else {
            pageCount++;
            tempDiv = document.createElement('div');
            pages.push(tempDiv);
            pages[pageCount].innerHTML += elem.innerHTML;
            currentSize = elem.offsetHeight;
            //console.log("dentro else, pagina " + pageCount);
        }
    });
    console.log("creazione pagine - N.pagine= " + pages.length)
    //creo le pagine in base alla divisione dell html fatta in precedenza
    for (let i = 0; i < pages.length; i++) {
        //console.log("pagina " + i);
        //console.log(pages[i]);
        await doc.html(pages[i], {
            callback: function (doc) {
                return doc;
            },
            x: xPosition,
            y: yPosition + pageHeight * (i),
            width: 570,
            windowWidth: 900,
            autoPaging: 'text',
            fontFaces: [{
                family: 'Material Icons',
                style: 'normal',
                weight: 'normal',
                src: [{
                    url: 'https://cdnjs.cloudflare.com/ajax/libs/mdui/0.1.2/icons/material-icons/MaterialIcons-Regular.ttf',
                    format: 'truetype'
                }]
            }]
        });
    }


    return new Promise((resolve, reject) => {
        // invoco il metodo di c# per settare la nuova dimensione dello schermo

        console.log("ciao");
        dotnet.invokeMethodAsync('DocumentCreated').then(() => {
            console.log("ciao2");
        }).catch(error => {
            console.log("Errore durante il ridimensionamento dello schermo: " + error);
        });
        //salvo il file
        doc.save(filename);
        resolve();
    });

}
