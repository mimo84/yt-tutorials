import { useEffect, useState } from 'react'
import { getFoods } from '../../../api/food'

const AddFood = () => {
  const [foodName, setFoodName] = useState('')

  const handleOnChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFoodName(e.target.value)
  }

  useEffect(() => {
    if (foodName.length > 2) {
      getFoods(foodName)
        .then((v) => {
          console.log(v.data)
        })
        .catch((e) => {
          console.log(e)
        })
    }
  }, [foodName])

  return (
    <>
      <div>Add Food Page</div>
      <input
        type="text"
        placeholder="Ricotta"
        value={foodName}
        onChange={handleOnChange}
      />
    </>
  )
}

export default AddFood
