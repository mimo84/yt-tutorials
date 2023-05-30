import fs from 'fs'

async function runAll() {
  const dir = fs.readdirSync('.')
  // const filename = '16.csv'
  for (const filename of dir) {

    if (filename.length === 6) {

      console.log('+++++++++++++++');
      console.log('running ', filename)
      const [dayNumberFromFileName] = filename.split('.')
      var ms = Date.parse(`2023-03-${dayNumberFromFileName}T00:00:00.000Z`)
      const jsDate = new Date(ms)

      const contents = fs.readFileSync(`./${filename}`, 'utf-8');
      const rows = contents.split('\n');
      const mealsOnDay = rows.reduce((prev, curr, index) => {
        if (index === 0) {
          return prev;
        }
        const [food_or_meal] = curr.split(',')
        if (food_or_meal.length === 0 || food_or_meal === 'Daily Totals' || food_or_meal === 'Goals') {
          return prev
        }

        if (['breakfast -', 'lunch -', 'snack 1 -', 'snack 2 -', 'dinner -'].includes(`${food_or_meal.split('-')[0]}-`.toLowerCase())) {
          const [meal_name_dirty] = food_or_meal.split('-')
          const meal_name = meal_name_dirty.trim()
          prev.current_meal = meal_name
          prev[meal_name] = []
        } else {
          const { current_meal } = prev;
          const [foodName, amount] = curr.split(',')

          prev[current_meal] = [...prev[current_meal], [foodName, amount]]
        }

        return prev;
      }, { current_meal: '' });


      const req = Object.keys(mealsOnDay).reduce((prev, curr) => {
        if (curr === 'current_meal') {
          return prev;
        }
        const foods = mealsOnDay[curr]
        if (!foods.length) {
          return prev;
        }

        if (!prev.mealEntries[curr]) {
          prev.mealEntries[curr] = {
            name: curr,
            foodEntries: []
          }
        }
        prev.mealEntries[curr].foodEntries = foods.map(([foodName, consumedAmount]) => {
          return {
            foodName,
            consumedAmount
          }
        })
        return prev;
      }, {
        date: jsDate.toISOString(),
        mealEntries: [

        ]
      })

      const finalJson = Object.keys(req.mealEntries).reduce((prev, curr) => {
        prev.push(req.mealEntries[curr])
        return prev;
      }, [])



      const r = await fetch("http://localhost:5066/diary/by_food_name", {
        "credentials": "include",
        "headers": {
          "User-Agent": "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:109.0) Gecko/20100101 Firefox/113.0",
          "Accept": "text/plain",
          "Accept-Language": "en-US",
          "Content-Type": "application/json",
          "Sec-Fetch-Dest": "empty",
          "Sec-Fetch-Mode": "cors",
          "Sec-Fetch-Site": "same-origin",
          "Sec-GPC": "1"
        },
        "referrer": "http://localhost:5066/swagger/index.html",
        "body": JSON.stringify({ date: jsDate.toISOString(), mealEntries: finalJson }),
        "method": "POST",
        "mode": "cors"
      });

      const res = await r.json();
      console.log(res);
    }
  }
}

await runAll()
