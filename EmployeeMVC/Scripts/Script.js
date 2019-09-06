function ShowImagePreview(imageUploader, previewImage, picturePath) {
    if (imageUploader.files && imageUploader.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.target.result);
            $(picturePath).val(imageUploader.files[0].name);
        }
        reader.readAsDataURL(imageUploader.files[0]);
        $(picturePath).val(imageUploader.files[0].name);
    }
}