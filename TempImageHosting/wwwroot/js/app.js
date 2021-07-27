const imageForm = document.querySelector("#imageForm");
const imageInput = document.querySelector("#imageInput");

imageForm.addEventListener("submit", handleSubmit);

async function handleSubmit(event) {
    event.preventDefault();
    const file = imageInput.files[0];
    const fileName = file.name;

    // get secure url from our server
    const uploadUrl = await getUploadUrl(fileName);
    if (uploadUrl === null) return;

    //console.log({ uploadUrl });

    // post the image direclty to the s3 bucket
    const uploadSuccess = await uploadImage(uploadUrl, file);
    if (!uploadSuccess) return;

    const imageUrl = uploadUrl.split("?")[0];
    //console.log({ imageUrl });

    const redirectUrl = "/home/success?imageUrl=" + imageUrl
    window.location.href = redirectUrl;
}

async function uploadImage(uploadUrl, file) {
    try {
        await fetch(uploadUrl, {
            method: "PUT",
            headers: {
                "Content-Type": "image/jpg"
            },
            body: file
        });

        return true;
    } catch (err) {
        console.error(err);
        return false;
    }
}

async function getUploadUrl(fileName) {
    try {
        return await fetch(`/api/s3?fileName=${fileName}`).then(res => res.text());
    }
    catch (err) {
        console.error(err);
        return null;
    }
}
