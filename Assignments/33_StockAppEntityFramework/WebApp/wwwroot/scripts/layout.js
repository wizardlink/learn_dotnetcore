const searchInput = document.querySelector("[name=stock-symbol]");

searchInput.addEventListener("keydown", (event) => {
    if (event.code === "Enter") window.location.href = `${event.target.value}`;
});
