    function saveAsFile(filename, data) {;
        const blob = b64toBlob(data, 'application/pdf');
    const link = document.createElement('a');
    link.href = window.URL.createObjectURL(blob);
    link.download = filename;
    link.click();
    }

    function b64toBlob(b64Data, contentType='', sliceSize=512) {
        const byteCharacters = atob(b64Data);
    const byteArrays = [];

    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
            const slice = byteCharacters.slice(offset, offset + sliceSize);
    const byteNumbers = new Array(slice.length);

    for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
            }

    const byteArray = new Uint8Array(byteNumbers);
    byteArrays.push(byteArray);
        }

    const blob = new Blob(byteArrays, {type: contentType });
    return blob;
    }
