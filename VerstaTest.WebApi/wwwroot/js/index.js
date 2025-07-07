window.onload = Load();

const keys = new Map([
	[0, "num"],
	[1, "sender_city"],
	[2, "sender_address"],
	[3, "recipient_city"],
	[4, "recipient_address"],
	[5, "weight"],
	[6, "date_cargo"],
]);

//#region CreatTable
function CreatRow(row) {
    let tr = document.createElement("tr");
	for (var key in row)
	{
		const value = document.createElement("td");

		if (key == "customerId") continue;

		if (key == "dateCargo")
		{
			value.innerHTML = new Date(row[key]).toLocaleString("ru");
		}
		else
		{
			value.innerHTML = row[key];
        }
        tr.append(value);
	}
	tr.addEventListener("click", e => {
		document.getElementById("num").removeAttribute("hidden");
		document.getElementById("num_h").removeAttribute("hidden");
		document.getElementById("addBtn").setAttribute("hidden", "hidden");
		document.getElementById("date_cargo").type = 'text';

		for (const [key, value] of keys)
		{
			document.getElementById(value).value = tr.cells[key].textContent;
			document.getElementById(value).setAttribute("readonly", "true");
		}

		window.location.href = '#zatemnenie';
	});
    return tr;
}
//#endregion

function OpenOrderForm() {
	document.getElementById("num").setAttribute("hidden", "hidden");
	document.getElementById("num_h").setAttribute("hidden", "hidden");
	document.getElementById("addBtn").removeAttribute("hidden");
	document.getElementById("date_cargo").type = 'date';

	for (const [key, value] of keys) {
		document.getElementById(value).value = null;
		document.getElementById(value).removeAttribute("readonly");
	}

	window.location.href = '#zatemnenie';
}

async function Load() {
	const params = new URLSearchParams({
		customerId: localStorage.getItem('userId')
	});

	const response = await fetch(`/order/home/get-orders-by-customer/?${params.toString()}`,
		{
			method: "GET",
			headers:
			{
				'Accept': 'application/json',
				'Content-Type': 'application/json'
			}
		});

	if (response.ok == true)
	{
		let rows = document.querySelector("tbody");
		answer = await response.json();
		orders = answer.orders;
		for (let i = 0; i < orders.length; i++)
		{
			rows.append(CreatRow(orders[i]));
		}
	}
}

function isNullOrEmpty(value) {
	return value == null || value === '';
}

async function CreatNewOrder() {
    let userId = localStorage.getItem('userId');
    let sender_city = document.getElementById("sender_city").value;
    let sender_address = document.getElementById("sender_address").value;
    let recipient_city = document.getElementById("recipient_city").value;
    let recipient_address = document.getElementById("recipient_address").value;
    let weight = document.getElementById("weight").value;
	let date_cargo = document.getElementById("date_cargo").value;

	if (isNullOrEmpty(userId) ||
		isNullOrEmpty(sender_city) ||
		isNullOrEmpty(sender_address) ||
		isNullOrEmpty(recipient_city) ||
		isNullOrEmpty(recipient_address) ||
		isNullOrEmpty(weight) ||
        isNullOrEmpty(date_cargo)) return alert("Пожалуйста, заполните все поля!");

	const response = await fetch("/order/home/create-order/", {
		method: "POST",
		headers: { "Accept": "application/json", "Content-Type": "application/json" },
		body: JSON.stringify({
			senderCity: sender_city,
			senderAddress: sender_address,
			recipientCity: recipient_city,
			recipientAddress: recipient_address,
			weight: weight,
			dateCargo: date_cargo,
			customerId: userId
		})
	});
	if (response.ok == true) {
		answer = await response.json();
		let rows = document.querySelector("tbody");
		rows.prepend(CreatRow(answer));
	}
	alert("Новый заказ добавлен в список!");
	window.location.href = '#';

	document.getElementById("sender_city").value = null;
	document.getElementById("sender_address").value = null;
	document.getElementById("recipient_city").value = null;
	document.getElementById("recipient_address").value = null;
	document.getElementById("weight").value = null;
	document.getElementById("date_cargo").value = null;
}