export function openCamera() {
    const player = document.getElementById('player');
    const canvas = document.getElementById('canvas');
    const shutterButton = document.getElementById('shutterButton');
    const controlButtons = document.getElementById('controlButtons');

    player.style.display = "block";
    shutterButton.style.display = "flex";
    canvas.style.display = "none";
    controlButtons.style.display = "none";

    const constraints = {
        video: {
            facingMode: 'environment',
            width: {ideal: 1920},
            height: {ideal: 1080}
        },
    };


    // Attach the video stream to the video element and autoplay.
    navigator.mediaDevices.getUserMedia(constraints).then((stream) => {
        
        canvas.height = stream.getVideoTracks()[0].getSettings().height;
        canvas.width =  stream.getVideoTracks()[0].getSettings().width;

        console.log("pre - canvas - height:"+ canvas.height + " width: " + canvas.width);
        console.log("pre - stream - height:"+ stream.getVideoTracks()[0].getSettings().height + " width: " + stream.getVideoTracks()[0].getSettings().width);  

        player.srcObject = stream;
    });

}

export async function takePicture() {
    const context = canvas.getContext('2d');

    console.log("post - height:"+ canvas.height + " width: " + canvas.width);   


    // Draw the video frame to the canvas.
    context.drawImage(player, 0, 0, canvas.width, canvas.height);
    //dotnethelper.invokeMethodAsync('ClientWebApp', 'GetImage', canvas.toDataURL())


    player.style.display = "none";
    canvas.style.display = "block";
    shutterButton.style.display = "none";
    controlButtons.style.display = "flex";
   
   
}

export function confirmPicture(dotnethelper)
{
    const context = canvas.getContext('2d');

    context.font = "30pt Arial";
    context.fillStyle = "white";

    let dateString = new Date().toLocaleString();

    if ("geolocation" in navigator) {
        navigator.geolocation.getCurrentPosition((position) => {
            context.fillText(`Posizione: ${position.coords.latitude} - ${position.coords.longitude}`, 20, 40 );  
            context.fillText("Data: " + dateString, 20, 80);
            const img = canvas.toDataURL();
            savePhoto(img, dotnethelper);
        });
    } else {
        //context.fillText("Posizione: non disponibile", 20, 40);
        context.fillText("Data: " + dateString, 20, 40);
        const img = canvas.toDataURL();
        savePhoto(img, dotnethelper);
    }
}

export function savePhoto(img, dotnethelper) {

    const stream = player.srcObject;
    stream.getTracks().forEach(function(track) {
        track.stop();
    });

    player.srcObject = null;


    dotnethelper.invokeMethodAsync('GetImage', img).then(() => {
    }).catch(error => {
        console.log("Errore immagine: " + error);
    });
}

export function openDocuments() {
    document.getElementById('pictureLoader').click();
}

// export function Init(dotnethelper) {
//     const player = document.getElementById('player');
//     const canvas = document.getElementById('canvas');
//     const context = canvas.getContext('2d');
//     const captureButton = document.getElementById('capture');

   
//     captureButton.addEventListener('click', () => {
//         // Draw the video frame to the canvas.
//         context.drawImage(player, 0, 0, canvas.width, canvas.height);
//         //dotnethelper.invokeMethodAsync('ClientWebApp', 'GetImage', canvas.toDataURL())

//         const stream = player.srcObject;
//         stream.getTracks().forEach(function(track) {
//             track.stop();
//           });

//         player.srcObject = null;


//         dotnethelper.invokeMethodAsync('GetImage', canvas.toDataURL()).then(() => {
//         }).catch(error => {
//             console.log("Errore immagine: " + error);
//         });
//     });

    
// }