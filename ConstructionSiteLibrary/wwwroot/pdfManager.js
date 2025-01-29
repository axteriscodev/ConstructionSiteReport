/**
 * Metodo usato per creare il PDF del documento compilato
 * @param {any} filename il nome del file
 * @returns il file pdf come download dal browser
 */
export async function generaPDFDocumento(
    filename,
    dotnet,
    documentString,
    compilatorString,
    committenteString,
    indirizzoCantiereString,
) {

    const doc = new jspdf.jsPDF({
        orientation: 'p',
        unit: 'pt',
        format: 'a4',
    });

    const addHeadersAndFooters = doc => {
        const pageCount = doc.internal.getNumberOfPages();
        for (let i = 1; i < pageCount + 1; i++) {
            doc.setPage(i);

            let x = 15; //posizione inizio disegno
            let y = 12; //posizione inizio disegno
            let rectx = 100; //Larghezza rettangolo 
            let recty = 10; //Altezza rettangolo

            const pageSize = doc.internal.pageSize;
            const pageWidth = pageSize.width ? pageSize.width : pageSize.getWidth();
            const pageHeight = pageSize.height ? pageSize.height : pageSize.getHeight();

            doc.setFontSize(8);
            doc.setTextColor(120, 120, 120);
            //Documento e 
            doc.text(documentString, x + 10, y + 10);
            doc.text(compilatorString, x + 10, y + recty + 10);

            //Committente ed indirizzo
            let xTitleComm = (pageWidth / 2) - 2;
            doc.text("Committente:" + committenteString, xTitleComm + 10, y + 10);
            doc.text("Indirizzo cantiere:" + indirizzoCantiereString, xTitleComm + 10, y + recty + 10);

            const footer = `Page ${i} of ${pageCount}`;

            console.log("pdfLog - pages: " + pageCount + ", actual: " + i);
            console.log("pdfLog - footer: " + footer);

            // Footer
            doc.text(footer, pageWidth / 2 - (doc.getTextWidth(footer) / 2), pageHeight - 15, {
                baseline: 'bottom'
            });
        }
    }

    const content = document.getElementById('printPDF').innerHTML;
    console.log("pdfLog - content offsetHeight: " + content.offsetHeight);
    //seleziono gli elementi che 
    const elements = document.querySelectorAll('.pdfElement');
    let headerY = 30; // Altezza header
    let footerY = 40 // altezza footer
    let yPosition = 15 + headerY;
    let xPosition = 12;
    let pageCount = 0;
    let currentSize = 0;
    let pageHeight = doc.internal.pageSize.getHeight();
    let pages = [];
    console.log("pdfLog - ph= " + pageHeight);
    let max = 1250 - headerY - footerY;

    let tempDiv = document.createElement('div');
    pages.push(tempDiv);
    //Ciclo ogni elemento per verificare se aggiungerlo in una pagina o creare una pagina nuova in base all'altezza
    elements.forEach((elem, index) => {
        console.log("pdfLog - elem= " + elem.innerHTML);

        let altezza = elem.offsetHeight + currentSize;
        if (altezza < max) {
            pages[pageCount].innerHTML += elem.innerHTML;
            currentSize = altezza;
        } else {
            pageCount++;
            tempDiv = document.createElement('div');
            pages.push(tempDiv);
            pages[pageCount].innerHTML += elem.innerHTML;
            currentSize = elem.offsetHeight;
        }
    });
    console.log("pdfLog - creazione pagine - N.pagine= " + pages.length)
    //creo le pagine in base alla divisione dell html fatta in precedenza
    for (let i = 0; i < pages.length; i++) {
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

    addHeadersAndFooters(doc);


    return new Promise((resolve, reject) => {
        // invoco il metodo di c# per settare la nuova dimensione dello schermo

        console.log("pdfLog - creazione...");
        dotnet.invokeMethodAsync('DocumentCreated').then(() => {
            console.log("pdfLog - creato!");
        }).catch(error => {
            console.log("pdfLog - Errore durante il ridimensionamento dello schermo: " + error);
        });

        //salvo il file
        doc.save(filename);
        resolve();
    });
}