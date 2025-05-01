const token = document.querySelector("#finnhub-token").innerHTML;

if (!token) throw new Error("Missing token value in #finnhub-token element.");

const socket = new WebSocket(`wss://ws.finnhub.io?token=${token}`);
const priceElement = document.querySelector("#stock-price");
const priceInputElement = document.querySelector("input[name=Price]");
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
        priceInputElement.value = tradeData.p;
    }
});
