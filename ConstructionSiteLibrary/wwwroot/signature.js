




/**
 * Metodo che inizializza il canvas in 2d e binda gli eventi del mouse
 * e del touch su di esso per disegnare
 * @param {any} canvasId
 */
export function loadCanvas(canvasId, dialogId) {

    console.log("dentro load canvas");
    console.log("id= " + dialogId);

    let canvas = document.getElementById(canvasId);
    let context = canvas.getContext('2d');
    let isIdle = true;
    function drawstart(event) {
        context.beginPath();
        var rect = canvas.getBoundingClientRect();
        context.moveTo(event.clientX - rect.left, event.clientY - rect.top);
        console.log("moveTo x: " + (event.clientX - rect.left));
        console.log("moveTo y: " + (event.pageY - (event.clientY - rect.top)));
        isIdle = false;
    }

    function drawmove(event) {
        if (isIdle) return;
        var rect = canvas.getBoundingClientRect();
        context.lineTo(event.clientX - rect.left, event.clientY - rect.top);
        console.log("moveTo x: " + (event.clientX - rect.left));
        console.log("moveTo y: " + (event.pageY - (event.clientY - rect.top)));

        context.stroke();
    }
    function drawend(event) {
        if (isIdle) return;
        drawmove(event);
        isIdle = true;
    }
    function touchstart(event) { drawstart(event.touches[0]) }
    function touchmove(event) { drawmove(event.touches[0]); event.preventDefault(); }
    function touchend(event) { drawend(event.changedTouches[0]) }

    canvas.addEventListener('touchstart', touchstart, false);
    canvas.addEventListener('touchmove', touchmove, false);
    canvas.addEventListener('touchend', touchend, false);

    canvas.addEventListener('mousedown', drawstart, false);
    canvas.addEventListener('mousemove', drawmove, false);
    canvas.addEventListener('mouseup', drawend, false);

    console.log("caricato");
}

/**
* ritorna true se tutti i color channel dei vari pixel è 0
* @param {any} canvas
* @returns
*/
export function isCanvasBlank(canvasId) {
    let canvas = document.getElementById(canvasId);
    let context = canvas.getContext('2d');

    let pixelBuffer = new Uint32Array(
        context.getImageData(0, 0, canvas.width, canvas.height).data.buffer
    );

    return pixelBuffer.some(color => color !== 0);
}

/**
 * Salva il contenuto del canvas come immagine in base64
 * @param {any} canvasId l'id del canvas
 * @returns l'immagine in formato base63
 */
export async function SaveCanvas(canvasId) {
    let canvas = document.getElementById(canvasId);    
    let img2 = await cropImage(canvas);
    return img2
}

async function cropImage(canvas) {
    let context = canvas.getContext('2d');
    //trasformo il contenuto del canvas in un array di uint32
    let data = new Uint32Array(context.getImageData(0, 0, canvas.width, canvas.height).data.buffer);
    //cerco nei 4 bordi da che righe e colonne parte la firma
    let top = scanUp(canvas.width, data);
    let bottom = scanDown(canvas.height, canvas.width, data);
    let left = scanLeft(canvas.width, data);
    let right = scanRight(canvas.width, data);
    //calcolo la nuova altezza e nuova larghezza togliendo righe e colonne vuote ai bordi
    let new_width = (right - left);
    let new_height = (bottom - top);
    //canvas non visualizzato utilizzato solo per creare l immagine tagliata
    let canvas2 = document.createElement('canvas');
    let context2 = canvas2.getContext('2d');
    canvas2.width = new_width;
    canvas2.height = new_height;
    //creo la bitmap del contenuto del canvas per poi tagliarlo nel secondo canvas
    let bitmap = await createImageBitmap(canvas);
    context2.drawImage(bitmap, left, top, new_width, new_height, 0, 0, new_width, new_height);
    
    let img = canvas2.toDataURL('image/png');
    return { image: img, width: new_width, height: new_height };
}

function scanUp(width,data) {

    var count = 0;

    let found = false;
    let max = width;
    let x = 0;
    while (!found) {
        for (x; x < max; x ++) {
            if (data[x] != 0) {
                found = true;
            }
        }
        if (!found) {
            count++;
            max += width;
            found = max >= data.length;
        }
    }
    return count;
}

function scanDown(height, width, data) {

    var count = height;

    let found = false;
    let max = data.length - width;
    let x = data.length - 1;
    while (!found) {
        for (x; x > max; x--) {
            if (data[x] != 0) {
                found = true;
            }
        }
        if (!found) {
            count--;
            max -= width;
            found = max < 0;
        }
    }
    return count;
}

function scanLeft(width, data) {

    var count = 0;

    let found = false;
    let max = data.length;

    while (!found) {
        let x = count;
        for (x; x < max; x += width) {
            if (data[x] != 0) {
                found = true;
            }
        }
        if (!found) {
            count++;
            found = count >= width;
        }
    }
    return count;
}

function scanRight(width, data) {

    var count = width;

    let found = false;
    let min = 0;
    let index = data.length-1;
    while (!found) {
        let x = index;
        for (x; x > min; x -= width) {
            if (data[x] != 0) {
                found = true;
            }
        }
        if (!found) {
            count--;
            index--;
            found = index <= 0;
        } 
    }
    return count;
}

