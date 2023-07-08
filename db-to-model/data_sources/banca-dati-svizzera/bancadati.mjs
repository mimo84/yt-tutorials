import fs from 'fs'
import csv from 'csv-parser';

let csvString = '';
const readStream = fs.createReadStream('banca-dati-dei-valori-nutritivi.csv', { encoding: 'utf-8' })
  .pipe(csv());
readStream.on('data', async (data) => {
  const [nome, cat, , , , , fat, sat, , , cholesterol, carbohydrates, sugar, , fiber, protein, , alcohol, , , , , , , , , , , , , , , , , potassium, sodium, calcium, , , iron] = Object.keys(data);

  const proteinDto = /[a-zA-Z<>]/gi.test(data[protein]) ? 0 : data[protein];
  const fatDto = /[a-zA-Z<>]/gi.test(data[fat]) ? 0 : data[fat];
  const carbohydratesDto = /[a-zA-Z<>]/gi.test(data[carbohydrates]) ? 0 : data[carbohydrates];
  const fiberDto = /[a-zA-Z<>]/gi.test(data[fiber]) ? 0 : data[fiber];
  const alcoholDto = /[a-zA-Z<>]/gi.test(data[alcohol]) ? 0 : data[alcohol];
  const sugarDto = /[a-zA-Z<>]/gi.test(data[sugar]) ? 0 : data[sugar];
  const saturatedFatsDto = /[a-zA-Z<>]/gi.test(data[sat]) ? 0 : data[sat];
  const sodiumDto = /[a-zA-Z<>]/gi.test(data[sodium]) ? 0 : data[sodium];
  const cholesterolDto = /[a-zA-Z<>]/gi.test(data[cholesterol]) ? 0 : data[cholesterol];
  const potassiumDto = /[a-zA-Z<>]/gi.test(data[potassium]) ? 0 : data[potassium];
  const ironDto = /[a-zA-Z<>]/gi.test(data[iron]) ? 0 : data[iron];
  const calciumDto = /[a-zA-Z<>]/gi.test(data[calcium]) ? 0 : data[calcium];

  // await new Promise(r => setTimeout(r, 200));

  // const request = {
  //   "body": {
  //     "food": {
  //       "name": `${data[nome]} ${data[cat]}`,
  //       "foodAmount": {
  //         "amount": 100,
  //         protein: proteinDto,
  //         fat: fatDto,
  //         carbohydrates: carbohydratesDto,
  //         fiber: fiberDto,
  //         alcohol: alcoholDto,
  //         sugar: sugarDto,
  //         saturatedFats: saturatedFatsDto,
  //         sodium: sodiumDto,
  //         cholesterol: cholesterolDto,
  //         potassium: potassiumDto,
  //         iron: ironDto,
  //         calcium: calciumDto,

  //         "source": "valorinutritivi.ch",
  //         "amountName": `100 - ${data[nome]}`
  //       }
  //     }
  //   }
  // }

  // Food Name	Amount	Protein	Fat	Carbs	Fiber	Alcohol	Sugar	Saturated Fats	Sodium	Cholesterol	Potassium	Iron	Calcium	Source	Price	Package Size	Package Size Parsed
  csvString += `${data[nome]} ${data[cat]}\t100\t${proteinDto}\t${fatDto}\t${carbohydratesDto}\t${fiberDto}\t${alcoholDto}\t${sugarDto}\t${saturatedFatsDto}\t${sodiumDto}\t${cholesterolDto}\t${potassiumDto}\t${ironDto}\t${calciumDto}\tvalorinutritivi.ch\n`;


  // const x = await fetch("http://localhost:5066/food/new", {
  //   "credentials": "include",
  //   "headers": {
  //     "User-Agent": "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:109.0) Gecko/20100101 Firefox/114.0",
  //     "Accept": "text/plain",
  //     "Accept-Language": "en-US",
  //     "Content-Type": "application/json",
  //     "Sec-Fetch-Dest": "empty",
  //     "Sec-Fetch-Mode": "cors",
  //     "Sec-Fetch-Site": "same-origin",
  //     "Sec-GPC": "1"
  //   },
  //   "referrer": "http://localhost:5066/swagger/index.html",
  //   "body": JSON.stringify(request),
  //   "method": "POST",
  //   "mode": "cors"
  // });

  // let y;
  // try {
  //   y = await x.json()
  // } catch (e) {
  //   console.log(e.message);
  //   console.log(y);
  //   console.log(x);
  // }

  // if (y.status === 400) {
  //   console.log(y);
  //   console.log(JSON.stringify(request, null, 2))
  // }


})
  .on('end', () => {

    fs.writeFileSync('banca-dati.csv', csvString);

  });

