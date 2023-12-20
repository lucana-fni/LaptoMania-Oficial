// qr-code-generator.js

function generateQRCode(elementId, text, width, height) {
    var qrcode = new QRCode(elementId, {
        text: text,
        width: width,
        height: height
    });
}
