/**
 * Metodo usato per creare il PDF del documento compilato
 * @param {any} filename il nome del file
 * @returns il file pdf come download dal browser
 */
export async function generaPDFDocumento(
    filename,
    dotnet,
    templateString, 
    compilatorString,
    committenteString,
    indirizzoCantiereString,
) {

    const doc = new jspdf.jsPDF({
        orientation: 'p',
        unit: 'pt',
        format: 'a4',
    });

    const addFooters = doc => {
        const pageCount = doc.internal.getNumberOfPages();
        for (let i = 1; i <= pageCount; i++) {
            doc.setPage(i);

            let x = 15;
            let y = 20;


            //Template
            doc.setDrawColor(192, 192, 192);
            doc.setFillColor(210, 210, 210);
            doc.rect(x, y, 60, 15, 'FD');
            doc.text(templateString, x + 10, y + 10);


            const pageSize = doc.internal.pageSize;
            const pageWidth = pageSize.width ? pageSize.width : pageSize.getWidth();
            const pageHeight = pageSize.height ? pageSize.height : pageSize.getHeight();
            const footer = `Page ${i} of ${pageCount}`;


            console.log("pdfLog - pageWidth: " + pageWidth);
            console.log("pdfLog - pageHeight: " + pageHeight);
            console.log("pdfLog - footer: " + footer);
            console.log("pdfLog - alt: " + pageHeight - 15);
            console.log("pdfLog - largh: " + pageWidth / 2 - (doc.getTextWidth(footer) / 2));

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
    let yPosition = 15 + headerY;
    let xPosition = 12;
    let pageCount = 0;
    let currentSize = 0;
    let pageHeight = doc.internal.pageSize.getHeight();
    let pages = [];
    console.log("pdfLog - ph= " + pageHeight);
    let max = 1350 - headerY;

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

        addFooters(doc);
    }


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