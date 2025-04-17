const token = document.querySelector("#finnhub-token").innerHTML;

if (!token) throw new Error("Missing token value in #finnhub-token element.");

const socket = new WebSocket(`wss://ws.finnhub.io?token=${token}`);
const priceElement = document.querySelector("#stock-price");
const stockSymbol = document.querySelector(".stock-symbol").innerHTML;

socket.addEventListener("open", (_event) => {
    socket.send(
        JSON.stringify({
            type: "subscribe",
            symbol: stockSymbol,
        }),
    );
});

socket.addEventListener("message", (event) => {
    const data = JSON.parse(event.data);

    if (data?.type === "ping") return;

    if (data?.type === "trade") {
        const tradeData = data.data[0];

        priceElement.innerHTML = tradeData.p;
    }
});

const orderQuantity = document.querySelector("[name=order-quantity]");
const sellButton = document.querySelector(".sell-button");
const buyButton = document.querySelector(".buy-button");
const stockName = document.querySelector(".stock-name").innerHTML;

const buildQuery = () =>
    [
        `price=${priceElement.innerHTML}`,
        `quantity=${orderQuantity.value}`,
        `stockName=${stockName}`,
        `stockSymbol=${stockSymbol}`,
    ].join("&");

buyButton.addEventListener("click", () => {
    window.location.href = `buyorder?${buildQuery()}`;
});

sellButton.addEventListener("click", () => {
    window.location.href = `sellorder?${buildQuery()}`;
});
