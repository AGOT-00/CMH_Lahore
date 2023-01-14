
const display = document.querySelector(".display");
const controllerWrapper = document.querySelector(".controllers");

const State = ["Initial", "Record", "Download"];
let stateIndex = 0;
let mediaRecorder,
    glstream,
    glblob = null;
(chunks = []), (audioURL = "");

var upload = document.getElementById("recsubmit");
upload.setAttribute("onclick", "voicevalidate()");

// mediaRecorder setup for audio
if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
    console.log("mediaDevices supported..");

    navigator.mediaDevices
        .getUserMedia({
            audio: true,
        })
        .then((stream) => {
            mediaRecorder = new MediaRecorder(stream);
            glstream = stream;
            mediaRecorder.ondataavailable = (e) => {
                chunks.push(e.data);
            };

            mediaRecorder.onstop = () => {
                const blob = new Blob(chunks, { type: "audio/ogg; codecs=opus" });
                glblob = blob;
                chunks = [];
                audioURL = window.URL.createObjectURL(blob);
                document.querySelector("audio").src = audioURL;
                uploadlink(blob);
            };
        })
        .catch((error) => {
            console.log("Following error has occured : ", error);
        });
} else {
    stateIndex = "";
    application(stateIndex);
}

const clearDisplay = () => {
    display.textContent = "";
};

const clearControls = () => {
    controllerWrapper.textContent = "";
};

const record = () => {
    upload.setAttribute("onclick", "voicevalidate()");

    stateIndex = 1;
    mediaRecorder.start();
    application(stateIndex);
};

const stopRecording = () => {
    //upload.setAttribute("onclick", "voicevalidate()");

    stateIndex = 2;
    mediaRecorder.stop();
    application(stateIndex);
};

function stopMic() {
    if (glstream) {
        glstream.getTracks().forEach((track) => track.stop());
    }
}

const addButton = (id, funString, text) => {
    const btn = document.createElement("button");
    btn.id = id;
    btn.setAttribute("onclick", funString);

    btn.textContent = text;
    controllerWrapper.append(btn);
};

const addMessage = (text) => {
    const msg = document.createElement("p");
    msg.textContent = text;
    display.append(msg);
};

const addAudio = () => {
    const audio = document.createElement("audio");
    audio.controls = true;
    audio.src = audioURL;
    audio.id = "audio";
    audio.classList.add("w-full");
    //audio.classList.push('w-full');
    display.append(audio);
};

function wait(ms) {
    var start = new Date().getTime();
    var end = start;
    while (end < start + ms) {
        end = new Date().getTime();
    }
}

function stopvoicesubmission() {
    stopMic();
    var recbtn = document.getElementById("record");
    recbtn.disabled = true;
}

function uploadtophp() {
    stopvoicesubmission();

    var form = document.getElementById("CMHComplaintForm");
    var formData = new FormData(form);
    formData.append("audio_data", glblob, "VoiceData");
    console.log("In Voice Section");
    fetch("/Home/SubmitVoicedata", {
        method: 'POST',
        body: formData,
    })
        .then(response => response.json())
        .then(data => {
            console.log(data);
            if (data==1) {
                window.location.replace("home/");
            }
        })
        .catch(error => {
            console.log(error);
            // Handle the error here
        });
}

function voicevalidate() {
    alert("Kindly record the voice first.\nبراہ کرم پہلے آواز ریکارڈ کریں۔");
}

function uploadlink(blob) {
    upload.setAttribute("onclick", "uploadtophp()");
}

const application = (index) => {
    switch (State[index]) {
        case "Initial":
            clearDisplay();
            clearControls();

            addButton("record", "record()", "Start Recording");
            break;

        case "Record":
            clearDisplay();
            clearControls();

            addMessage("Recording...");
            // add class to display
            display.classList.add("mb-10");
            addButton("stop", "stopRecording()", "Stop Recording");
            break;

        case "Download":
            clearControls();
            clearDisplay();

            addAudio();
            addButton("record", "record()", "Record Again");
            break;

        default:
            clearControls();
            clearDisplay();

            addMessage("Your browser does not support mediaDevices");
            break;
    }
};

application(stateIndex);