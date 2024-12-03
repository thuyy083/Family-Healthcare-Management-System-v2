const connection = new signalR.HubConnectionBuilder()
    .withUrl("/tuVanHub")
    .build();

// Kết nối đến Hub
connection.start().catch(err => console.error(err.toString()));

// Lắng nghe sự kiện 'ReceiveMessage' từ server
connection.on("ReceiveMessage", (maBacSi, maKhachHang, noiDungTinNhan) => {
    // Hiển thị tin nhắn mới trên giao diện
    const messageElement = document.createElement("div");
    messageElement.textContent = `Bác sĩ ${maBacSi} gửi tin nhắn: ${noiDungTinNhan}`;
    document.getElementById("messagesList").appendChild(messageElement);
});

// Hàm gửi tin nhắn
document.getElementById("sendButton").addEventListener("click", () => {
    const maBacSi = document.getElementById("maBacSi").value;
    const maKhachHang = document.getElementById("maKhachHang").value;
    const noiDungTinNhan = document.getElementById("noiDungTinNhan").value;

    // Gửi yêu cầu tới controller để xử lý gửi tin nhắn
    fetch("/TuVan/SendMessage", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ maBacSi, maKhachHang, noiDungTinNhan })
    }).catch(err => console.error(err.toString()));
});
