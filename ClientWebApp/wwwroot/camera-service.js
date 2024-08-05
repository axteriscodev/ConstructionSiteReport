export function openCamera() {
  const player = document.getElementById("player");
  const canvas = document.getElementById("canvas");
  const shutterButton = document.getElementById("shutterButton");
  const controlButtons = document.getElementById("controlButtons");

  player.style.display = "block";
  shutterButton.style.display = "flex";
  canvas.style.display = "none";
  controlButtons.style.display = "none";

  const constraints = {
    video: {
      facingMode: "environment",
      width: { ideal: 1920 },
      height: { ideal: 1080 },
    },
  };

  // Attach the video stream to the video element and autoplay.
  navigator.mediaDevices.getUserMedia(constraints).then((stream) => {
    canvas.height = stream.getVideoTracks()[0].getSettings().height;
    canvas.width = stream.getVideoTracks()[0].getSettings().width;

    console.log(
      "pre - canvas - height:" + canvas.height + " width: " + canvas.width
    );
    console.log(
      "pre - stream - height:" +
        stream.getVideoTracks()[0].getSettings().height +
        " width: " +
        stream.getVideoTracks()[0].getSettings().width
    );

    player.srcObject = stream;
  });
}

export async function takePicture() {
  const context = canvas.getContext("2d");

  console.log("post - height:" + canvas.height + " width: " + canvas.width);

  // Draw the video frame to the canvas.
  context.drawImage(player, 0, 0, canvas.width, canvas.height);
  //dotnethelper.invokeMethodAsync('ClientWebApp', 'GetImage', canvas.toDataURL())

  player.style.display = "none";
  canvas.style.display = "block";
  shutterButton.style.display = "none";
  controlButtons.style.display = "flex";
}

export function confirmPicture(dotnethelper) {
  const context = canvas.getContext("2d");

  console.log("prima di geolocalization");
  if ("geolocation" in navigator) {
    console.log("geolocalization on");
    navigator.geolocation.getCurrentPosition(
      (position) => {
        let geotext = `Posizione: ${position.coords.latitude} - ${position.coords.longitude}`;
        createGeolabel(context, geotext, dotnethelper);
      },
      (error) => {
        let geotext = "Posizione: non disponibile";
        createGeolabel(context, geotext, dotnethelper);
      }
    );
  } else {
    let geotext = "Posizione: non disponibile";
    createGeolabel(context, geotext, dotnethelper);
  }
}

export function createGeolabel(context, geotext, dotnethelper) {
  context.font = "30pt Arial";

  const text = context.measureText(geotext);
  console.log("larghezza: " + text.width + 5, ", altezza: " + text.height + 5);
  const textWidth = text.actualBoundingBoxRight + text.actualBoundingBoxLeft;

  let dateString = new Date().toLocaleString();

  var biggest = (485 > textWidth ? 485 : textWidth)

  context.fillStyle = "rgba(50, 50, 50, 0.8)";
  context.fillRect(15, 5, biggest + 15, 85);

  context.fillStyle = "white";

  context.fillText(geotext, 20, 40);
  context.fillText("Data: " + dateString, 20, 80);

  const img = canvas.toDataURL();
  savePhoto(img, dotnethelper);
}

export function savePhoto(img, dotnethelper) {
  const stream = player.srcObject;
  stream.getTracks().forEach(function (track) {
    track.stop();
  });

  player.srcObject = null;

  dotnethelper
    .invokeMethodAsync("GetImage", img)
    .then(() => {})
    .catch((error) => {
      console.log("Errore immagine: " + error);
    });
}

export function openDocuments() {
  document.getElementById("pictureLoader").click();
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
