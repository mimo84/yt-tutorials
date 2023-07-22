import { CalendarIcon } from '@heroicons/react/20/solid'
import { CakeIcon } from '@heroicons/react/24/outline'

const DiaryHeading = () => {
  return (
    <div className="lg:flex lg:items-center lg:justify-between">
      <div className="min-w-0 flex-1">
        <h2>Diary</h2>
      </div>
      <div className="min-w-0 flex-1">
        <div className="mt-1 flex flex-col sm:mt-0 sm:flex-row sm:flex-wrap sm:space-x-6">
          <div className="mt-2 flex items-center text-sm text-gray-500">
            <CakeIcon
              className="mr-1.5 h-5 w-5 flex-shrink-0 text-gray-400"
              aria-hidden="true"
            />
            1200kcal/2100kcal
          </div>

          <div className="mt-2 flex items-center text-sm text-gray-500">
            <CalendarIcon
              className="mr-1.5 h-5 w-5 flex-shrink-0 text-gray-400"
              aria-hidden="true"
            />
            22nd July 2023
          </div>
        </div>
      </div>
    </div>
  )
}

export default DiaryHeading
