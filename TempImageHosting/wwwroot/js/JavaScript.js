const imageForm = document.querySelector("#imageForm");
const imageInput = document.querySelector("#imageInput");

imageForm.addEventListener("submit", async (event) => {
    event.preventDefault();
    const file = imageInput.files[0];
    const fileName = file.name;

    // get secure url from our server
    const url = await fetch(`/api/s3?fileName=${fileName}`).then(res => res.text());
    console.log({ url });

    // post the image direclty to the s3 bucket
    await fetch(url, {
        method: "PUT",
        headers: {
            "Content-Type": "multipart/form-data"
        },
        body: file
    });

    const imageUrl = url.split("?")[0];
    console.log({ imageUrl });


    window.location.href = "/home/success?imageUrl=" + imageUrl;
});
