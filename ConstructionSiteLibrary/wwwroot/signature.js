
/**
 * Metodo che inizializza il canvas in 2d e binda gli eventi del mouse
 * e del touch su di esso per disegnare
 * @param {any} canvasId
 */
export function loadCanvas(canvasId) {

    let canvas = document.getElementById(canvasId);
    let context = canvas.getContext('2d');
    let isIdle = true;
    function drawstart(event) {
        context.beginPath();
        context.moveTo(event.pageX - canvas.offsetLeft, event.pageY - canvas.offsetTop);
        isIdle = false;
    }
    function drawmove(event) {
        if (isIdle) return;
        context.lineTo(event.pageX - canvas.offsetLeft, event.pageY - canvas.offsetTop);
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
export function SaveCanvas(canvasId) {
    let canvas = document.getElementById(canvasId);    
    let img2 = cropImage(canvas);
    return img2
}

function cropImage(canvas) {
    let context = canvas.getContext('2d');
    //let data = context.getImageData(0, 0, oldWidth, oldHeight).data.buffer;
    let data = new Uint32Array(context.getImageData(0, 0, canvas.width, canvas.height).data.buffer);
    console.log("w: " + canvas.width + " h: " + canvas.height);
    let top = scanUp(canvas.width, data);
    let bottom = scanDown(canvas.height, canvas.width, data);
    let left = scanLeft(canvas.width, data);
    let right = scanRight(canvas.width, data);
    console.log(right + " - "+  left);
    let new_width = (right - left);
    let new_height = (bottom - top);

    let canvas2 = document.createElement('canvas');
    let context2 = canvas2.getContext('2d');
    //canvas2.style.width = new_width;
    //canvas2.style.height = new_height;
    console.log("new-w: " + new_width + " new-h: " + new_height);
    context2.drawImage(canvas, left, top, new_width, new_height, 0, 0, new_width, new_height);
    //let img = canvas2.toDataURL('image/png');
    //return { image: img, width: new_width, height: new_height };
    let img = canvas.toDataURL('image/png');
    return { image: img, width: canvas.width, height: canvas.height };

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
    console.log("count top " + count);
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
    console.log("count bottom " + count);
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
                //console.log("pixel left " + data[x]);
                //console.log("x left " + x);
            }
        }
        if (!found) {
            count++;
            //console.log("count " + count);
            found = count >= width;
        }
    }
    console.log("count left" + count);
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
                //console.log("pixel right " + data[x]);
                //console.log("x right " + x);
            }
        }
        if (!found) {
            count--;
            index--;
            //console.log("count " + count);
            found = index <= 0;
        } 
    }
    console.log("count right " + count);
    return count;
}

