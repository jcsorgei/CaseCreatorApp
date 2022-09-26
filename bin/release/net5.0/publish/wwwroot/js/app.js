let svg;
let currentTemplate;
let currentObject;
let images = [];
let texts = [];
let templates = [];
let rotateInput = document.getElementById("rotateInput");
let scaleInput = document.getElementById("scaleInput");
let nameInput = document.getElementById("nameInput");
let textInput = document.getElementById("textInput");
let templateSelect = document.getElementById("device");
let fontSelect = document.getElementById("font");
let fontColorSelect = document.getElementById("fontColor");

let mode = "create";
let currentId;

function load() {

    let svgdoc = document.getElementById("svgdoc").contentDocument;

    if (svgdoc == null) {
        setTimeout(load, 100);
        return;
    }

    svg = svgdoc.getElementById("svg5");
    // templates = Array.from(svgdoc.querySelectorAll("g")).map(t => new Template(t));

    templates = [
        new Template(
            0,
            svg.getElementById("iphone12_device"),
            svg.getElementById("iphone12_camera")
        ),
        new Template(
            1,
            svg.getElementById("s21ultra_device"),
            svg.getElementById("s21ultra_camera")
        ),
        new Template(
            2,
            svg.getElementById("note20ultra_device"),
            svg.getElementById("note20ultra_camera")
        ),
        new Template(
            3,
            svg.getElementById("nintendoswitch_device"),
            null
        )
    ]

    selectTemplate(0);
    loadFonts();

    svg.addEventListener("keydown", (e) => {
        if (e.key == "Delete") {
            console.log("deletetest")
            currentObject.delete();
        }
    })

    getSavedSvgs();
}

load();


function loadFonts() {
    let fontStyles = ["serif", "sans-serif", "cursive", "fantasy", "monospace", "system-ui", "emoji", "math", "fangsong", "ui-serif", "ui-sans-serif", "ui-monospace",
        "ui-rounded"
    ];

    fontStyles.forEach(font => {
        let option = document.createElement("option");
        option.value = font;
        option.innerHTML = font;
        option.style.fontFamily = font;
        fontSelect.add(option);
    })
}

function selectImage() {
    let imgSrc = document.getElementById("imgSrc");
    // let filePath = URL.createObjectURL(imgSrc.files[0]);
    getBase64(imgSrc.files[0]).then(data => {
        let newImg = new SvgImage(data);
        newImg.clip();
        images.push(newImg);
    });
    // let filePath = imageToBase64(imgSrc);
    // test = filePath;

    // let newImg = new SvgImage(objCounter, filePath);
    // objCounter++;
    // newImg.clip();
    // svgObjects.push(newImg);

}

function getBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });
}

function selectTemplate(index) {
    hideAllTemplates();
    resetTemplates();
    if (index != null) {
        currentTemplate = templates[index];
        currentTemplate.show();
        clipAllObjects();
    }

}

function hideAllTemplates() {
    templates.forEach(t => t.hide());
}

function resetTemplates() {
    templates.forEach(t => t.resetColor());
    images.forEach(img => img.delete2());
    texts.forEach(text => text.delete2());

    texts = [];
    images = [];

    nameInput.value = "";
    textInput.value = "";
    rotateInput.value = 0;
    scaleInput.value = 0;

}


function setColor(hexValue) {

    currentTemplate.setColor(hexValue);
}


function clipAllObjects() {
    images.forEach(image => {
        image.clip();
    });
    texts.forEach(text => {
        text.clip();
    });
}

function removeClipPath() {

    let clipPath = svg.getElementById("cp");
    if (clipPath != null) {
        clipPath.remove();
    }

}

function exportSvg() {
    svg.setAttribute("xmlns", "http://www.w3.org/2000/svg");
    let svgData = svg.outerHTML;
    let preface = '<?xml version="1.0" standalone="no"?>\r\n';
    let svgBlob = new Blob([preface, svgData], {
        type: "image/svg+xml;charset=utf-8"
    });
    let svgUrl = URL.createObjectURL(svgBlob);
    let downloadLink = document.createElement("a");
    downloadLink.href = svgUrl;
    downloadLink.download = "result.svg";
    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);
}

function sendSvgImage() {
    class SvgImageDTO {};
    let svgImageDto = new SvgImageDTO();

    var svgElement = document.getElementById('svgId');

    // Create your own image
    var img = document.createElement('img');

    // Serialize the svg to string
    var svgString = new XMLSerializer().serializeToString(svg);

    // Remove any characters outside the Latin1 range
    var decoded = unescape(encodeURIComponent(svgString));

    // Now we can use btoa to convert the svg to base64
    var base64 = btoa(decoded);

    var imgSource = `data:image/svg+xml;base64,${base64}`;

    img.setAttribute('src', imgSource);

    console.log(imgSource);

    // svgdoc.setAttribute("data", imgSource);

    var tempDiv = document.createElement("div");
    var cloneSvg = svg.cloneNode(true);
    tempDiv.appendChild(cloneSvg);

    var svgText = tempDiv.innerHTML;

    svgImageDto.templateId = currentTemplate.id;
    svgImageDto.templateColor = currentTemplate.color;
    svgImageDto.base64 = imgSource;
    svgImageDto.name = nameInput.value;
    svgImageDto.svg = svgText;
    svgImageDto.images = images.map(img => {
        return {
            base64: img.filePath,
            rotateDeg: img.rotateDeg,
            scaleFactor: img.scaleFactor,
            currentTransformation: img.currenttransformation
        }
    })

    svgImageDto.texts = texts.map(text => {
        return {
            textValue: text.textValue,
            color: text.color,
            fontStyle: text.fontStyle,
            rotateDeg: text.rotateDeg,
            scaleFactor: text.scaleFactor,
            currentTransformation: text.currenttransformation
        }
    })

    if (mode == "create") {
        $.post({
            url: "/SvgData",
            data: JSON.stringify(svgImageDto),
            contentType: "application/json",
            success: function () {
                resetTemplates();
                getSavedSvgs();
            }
        });
    }

    if (mode == "edit") {
        $.ajax({
            method: "PUT",
            url: `/SvgData/${currentId}`,
            data: JSON.stringify(svgImageDto),
            contentType: "application/json",
            success: function () {
                resetTemplates();
                getSavedSvgs();
            }

        })
    }


}

function createMode() {
    mode = 'create';
    resetTemplates();
}

function getSavedSvgs() {
    let imagesContainer = document.querySelector("#saved-images>.row");
    imagesContainer.innerHTML = "";
    $.get({
        url: "/SvgData",
        contentType: "application/json",
        success: function (data) {

            let createNewCard = `
                <div class="col-md-3 d-flex align-items-center justify-content-center">   
                    <button type="button" onclick='createMode()' class="btn btn-primary">Create New Case</button>
                </div>

            `
            imagesContainer.innerHTML += createNewCard;

            data.forEach(d => {
                let templateCard = `
                    <div class="col-md-3">
                        <div class="card mb-2 border-2" style="width: 18rem;">
                            <object width='200px' height='200px' class="card-img-top" id="svgdoc" data="${d.base64}"> </object>
                            <div class="card-body">
                            <h5 class="card-title">${d.name}</h5>
                            <button onclick='loadSvg(${d.id})' class="btn btn-sm btn-primary text-center">Edit</button>
                            <button onclick='deleteSvg(${d.id})' class="btn btn-sm btn-danger text-center">Delete</button>
                            </div>
                        </div>
                    </div>
                `
                imagesContainer.innerHTML += templateCard;
            })

        }
    })
}

function loadSvg(id) {
    console.log("load svg" + id);
    $.get({
        url: `/SvgData/${id}`,
        contentType: "application/json",
        success: function (data) {
            mode = "edit";
            currentId = id;
            resetTemplates();

            selectTemplate(data.templateId);
            templateSelect.options[data.templateId].selected = true;
            currentTemplate.setColor(data.templateColor);

            nameInput.value = data.name;

            data.images.forEach(img => {
                let svgImg = new SvgImage(img.base64);
                svgImg.updateTransform(img.rotateDeg, img.scaleFactor, img.currentTransformation);
                svgImg.rotateDeg = img.rotateDeg;
                svgImg.scaleFactor = img.scaleFactor;
                svgImg.lasttransformation = img.currentTransformation;
                svgImg.currenttransformation = img.currentTransformation;
                svgImg.clip();
                images.push(svgImg);
            });

            data.texts.forEach(text => {
                let svgText = new SvgText(text.textValue);
                svgText.updateTransform(text.rotateDeg, text.scaleFactor, text.currentTransformation);
                svgText.text.style.fill = text.color;
                svgText.text.style.fontFamily = text.fontStyle;
                svgText.rotateDeg = text.rotateDeg;
                svgText.scaleFactor = text.scaleFactor;
                svgText.lasttransformation = text.currentTransformation;
                svgText.currenttransformation = text.currentTransformation;
                svgText.clip();
                texts.push(svgText);
            });

        }
    })

}

function deleteSvg(id) {

    $.ajax({
        method: "DELETE",
        url: `/SvgData/${id}`,
        success: function (data) {
            resetTemplates();
            getSavedSvgs();
        }

    })
}

// function updateSvg(id) {
//     $.ajax({
//         method: "UPDATE",
//         url: `/SvgData/${id}`,
//         data: success: function (data) {
//             resetTemplates();
//             getSavedSvgs();
//         }

//     })
// }

function rotateHandle(deg) {
    if (currentObject != null) {
        currentObject.rotate(deg);
    }
}

function scaleHandle(multiplier) {
    if (currentObject != null) {
        currentObject.scale(multiplier);
    }
}


class SvgImage {
    constructor(filePath) {
        this.filePath = filePath;
        this.img = document.createElementNS("http://www.w3.org/2000/svg", "image");
        this.setImage();
        this.dragging = false;
        this.dragstartpos;
        this.lasttransformation = {
            x: 0,
            y: 0
        };
        this.currenttransformation = {
            x: 0,
            y: 0
        };
        this.mousepos;
        this.pt = svg.createSVGPoint();
        svg.appendChild(this.img);

        this.imgBox = this.img.getBoundingClientRect();
        this.centerx = this.imgBox.x + this.imgBox.width / 2;
        this.centery = this.imgBox.y + this.imgBox.height / 2;

        this.img.onmousedown = this.mouseDown.bind(this);
        this.img.onmouseup = this.mouseUp.bind(this);
        this.img.onclick = function () {
            console.log("click")
            this.setCurrentObject();
            scaleInput.value = this.scaleFactor;
            rotateInput.value = this.rotateDeg;
        }.bind(this);
        this.img.ondblclick = this.raise.bind(this);
        svg.addEventListener("mousemove", (pos) => {
            this.mouseMove(pos);
        })
    }

    setImage() {
        this.img.setAttribute("href", this.filePath);
        this.img.setAttribute("width", 200);
        this.img.setAttribute("height", 140);
        // this.img.setAttribute("crossorigin", "anonymous");

        let svgBox = svg.getBoundingClientRect();

        let cx = svgBox.x + svgBox.width / 2;
        let cy = svgBox.y + svgBox.height / 2;

        this.img.setAttribute("x", cx - 200 / 2);
        this.img.setAttribute("y", cy - 150 / 2);

    }

    setCurrentObject() {
        currentObject = this;
    }

    mouseDown() {
        console.log("mousedown");
        this.dragging = true;
        this.dragstartpos = this.mousepos;
    }

    mouseUp() {
        console.log("mouseup");
        this.dragging = false;
        this.lasttransformation = this.currenttransformation;
    }

    mouseMove(pos) {
        this.pt.x = pos.x;
        this.pt.y = pos.y;
        this.mousepos = this.pt.matrixTransform(svg.getScreenCTM().inverse());

        if (this.dragging) {
            var trf = {
                x: this.mousepos.x - this.dragstartpos.x + this.lasttransformation.x,
                y: this.mousepos.y - this.dragstartpos.y + this.lasttransformation.y
            };
            this.currenttransformation = trf;
            // this.img.setAttribute("transform", `translate(${trf.x},${trf.y})`)
            this.updateTransform(this.rotateDeg, this.scaleFactor, this.currenttransformation);

        }
    }

    rotateDeg = 0;
    scaleFactor = 1;

    transform(deg, multiplier, currenttransformation) {
        let tr1 = "translate(" + (-this.centerx) + "," + (-this.centery) + ") ";
        let rot = "rotate(" + deg + ")";
        let tr2 = "translate(" + (this.centerx) + "," + (this.centery) + ") ";


        let tr3 = "translate(" + (-this.centerx) + "," + (-this.centery) + ") ";
        let scale = `scale(${multiplier})`;
        let tr4 = "translate(" + (this.centerx) + "," + (this.centery) + ") ";

        let tr5 = `translate(${currenttransformation.x},${currenttransformation.y})`;

        return tr5 + tr2 + rot + tr1 + " " + tr4 + scale + tr3;
    }

    updateTransform(deg, multiplier, currenttransformation) {
        this.img.setAttribute("transform", this.transform(deg, multiplier, currenttransformation));
    }

    rotate(deg) {
        this.rotateDeg = deg;
        this.updateTransform(this.rotateDeg, this.scaleFactor, this.currenttransformation);
    }

    scale(multiplier) {
        this.scaleFactor = multiplier;
        this.updateTransform(this.rotateDeg, this.scaleFactor, this.currenttransformation);
    }

    raise() {
        console.log("raise");
        svg.appendChild(this.img);
        this.clip();
    }

    clip() {
        removeClipPath();
        let clipPath = document.createElementNS("http://www.w3.org/2000/svg", "clipPath");
        clipPath.id = "cp";
        svg.appendChild(clipPath);
        let templatePath = currentTemplate.devicePath.cloneNode(true);
        clipPath.appendChild(templatePath);
        let group = document.createElementNS("http://www.w3.org/2000/svg", "g");
        svg.appendChild(group);
        group.appendChild(this.img);
        group.setAttribute("clip-path", "url(#cp)");
        svg.appendChild(currentTemplate.cameraPath);
    }

    delete() {
        console.log("delete" + images.indexOf(this));
        images.splice(images.indexOf(this), 1);
        this.img.remove();

    }

    delete2() {
        this.img.remove();
    }

}

class Template {
    constructor(id, devicePath, cameraPath) {
        this.id = id;
        this.devicePath = devicePath;
        this.cameraPath = cameraPath;
        this.color = "none";
    }

    setColor(hexValue) {
        this.devicePath.style.fill = hexValue;
        this.color = hexValue;
    }

    resetColor() {
        this.devicePath.style.fill = "none";
        this.color = "none";
    }

    hide() {
        this.devicePath.style.display = "none";
        if (this.cameraPath != null) {
            this.cameraPath.style.display = "none";
        }
    }

    show() {
        this.devicePath.style.display = "inline";
        if (this.cameraPath != null) {
            this.cameraPath.style.display = "inline";
        }
    }

}

class SvgText {
    constructor(textValue) {
        this.textValue = textValue;
        this.color = fontColorSelect.value;
        this.fontStyle = fontSelect.value;
        this.text = document.createElementNS("http://www.w3.org/2000/svg", "text");
        this.setText();
        this.dragging = false;
        this.dragstartpos;
        this.lasttransformation = {
            x: 0,
            y: 0
        };
        this.currenttransformation = {
            x: 0,
            y: 0
        };
        this.mousepos;
        this.pt = svg.createSVGPoint();
        svg.appendChild(this.text);

        this.textBox = this.text.getBoundingClientRect();
        this.centerx = this.textBox.x + this.textBox.width / 2;
        this.centery = this.textBox.y + this.textBox.height / 2;

        this.text.onmousedown = this.mouseDown.bind(this);
        this.text.onmouseup = this.mouseUp.bind(this);
        this.text.onclick = function () {
            console.log("click");
            this.setCurrentObject();
            scaleInput.value = this.scaleFactor;
            rotateInput.value = this.rotateDeg;
        }.bind(this);
        this.text.ondblclick = this.raise.bind(this);
        svg.addEventListener("mousemove", (pos) => {
            this.mouseMove(pos);
        })
    }

    setText() {
        this.text.textContent = this.textValue;
        this.text.setAttribute("width", 200);
        this.text.setAttribute("height", 140);
        this.text.style.fill = this.color;
        this.text.style.fontFamily = this.fontStyle;


        let svgBox = svg.getBoundingClientRect();

        let cx = svgBox.x + svgBox.width / 2;
        let cy = svgBox.y + svgBox.height / 2;

        this.text.setAttribute("x", cx - 200 / 2);
        this.text.setAttribute("y", cy - 150 / 2);

    }

    setCurrentObject() {
        currentObject = this;
    }

    mouseDown() {
        this.dragging = true;
        this.dragstartpos = this.mousepos;
    }

    mouseUp() {
        this.dragging = false;
        this.lasttransformation = this.currenttransformation;
    }

    mouseMove(pos) {
        this.pt.x = pos.x;
        this.pt.y = pos.y;
        this.mousepos = this.pt.matrixTransform(svg.getScreenCTM().inverse());

        if (this.dragging) {
            var trf = {
                x: this.mousepos.x - this.dragstartpos.x + this.lasttransformation.x,
                y: this.mousepos.y - this.dragstartpos.y + this.lasttransformation.y
            };
            this.currenttransformation = trf;
            // this.img.setAttribute("transform", `translate(${trf.x},${trf.y})`)
            this.updateTransform(this.rotateDeg, this.scaleFactor, this.currenttransformation);

        }
    }

    rotateDeg = 0;
    scaleFactor = 1;

    transform(deg, multiplier, currenttransformation) {
        let tr1 = "translate(" + (-this.centerx) + "," + (-this.centery) + ") ";
        let rot = "rotate(" + deg + ")";
        let tr2 = "translate(" + (this.centerx) + "," + (this.centery) + ") ";


        let tr3 = "translate(" + (-this.centerx) + "," + (-this.centery) + ") ";
        let scale = `scale(${multiplier})`;
        let tr4 = "translate(" + (this.centerx) + "," + (this.centery) + ") ";

        let tr5 = `translate(${currenttransformation.x},${currenttransformation.y})`;

        return tr5 + tr2 + rot + tr1 + " " + tr4 + scale + tr3;
    }

    updateTransform(deg, multiplier, currenttransformation) {
        this.text.setAttribute("transform", this.transform(deg, multiplier, currenttransformation));
    }

    rotate(deg) {
        this.rotateDeg = deg;
        this.updateTransform(this.rotateDeg, this.scaleFactor, this.currenttransformation);
    }

    scale(multiplier) {
        this.scaleFactor = multiplier;
        this.updateTransform(this.rotateDeg, this.scaleFactor, this.currenttransformation);
    }

    raise() {
        console.log("raise");
        svg.appendChild(this.text);
        this.clip();
    }

    clip() {
        removeClipPath();
        let clipPath = document.createElementNS("http://www.w3.org/2000/svg", "clipPath");
        clipPath.id = "cp";
        svg.appendChild(clipPath);
        let templatePath = currentTemplate.devicePath.cloneNode(true);
        clipPath.appendChild(templatePath);
        let group = document.createElementNS("http://www.w3.org/2000/svg", "g");
        svg.appendChild(group);
        group.appendChild(this.text);
        group.setAttribute("clip-path", "url(#cp)");
        svg.appendChild(currentTemplate.cameraPath);
    }

    delete() {
        texts.splice(texts.indexOf(this), 1);
        this.text.remove();
    }

    delete2() {
        this.text.remove();
    }

}


function addText() {
    if (textInput.value) {
        let newText = new SvgText(textInput.value);
        newText.clip();
        texts.push(newText);
    }

}