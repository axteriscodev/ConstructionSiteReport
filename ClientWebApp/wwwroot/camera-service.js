export function openCamera() {
    const player = document.getElementById('player');

    const constraints = {
        video: true,
    };


    // Attach the video stream to the video element and autoplay.
    navigator.mediaDevices.getUserMedia(constraints).then((stream) => {
        player.srcObject = stream;
    });

}

export function takePicture(dotnethelper) {
    const canvas = document.getElementById('canvas');
    const context = canvas.getContext('2d');

     // Draw the video frame to the canvas.
     context.drawImage(player, 0, 0, canvas.width, canvas.height);
     //dotnethelper.invokeMethodAsync('ClientWebApp', 'GetImage', canvas.toDataURL())

     context.font = "10pt Arial";
     context.fillStyle = "white";

     if ("geolocation" in navigator) {
        navigator.geolocation.getCurrentPosition((position) => {
            context.fillText(`Posizione: ${position.coords.latitude} - ${position.coords.longitude}`, 20, 20 )
            //doSomething(position.coords.latitude, position.coords.longitude);
            
            const stream = player.srcObject;
            stream.getTracks().forEach(function(track) {
                track.stop();
            });

            player.srcObject = null;


            dotnethelper.invokeMethodAsync('GetImage', canvas.toDataURL()).then(() => {
            }).catch(error => {
                console.log("Errore immagine: " + error);
            });
        });
      } else {
        context.fillText("Posizione: non disponibile", 20, 20);
      }

     

     

     
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