import PageBody from '../../components/Layout/PageBody'
import PageHeading from '../../components/Layout/PageHeading'
import { Link, Outlet } from 'react-router-dom'
import DiaryHeading from '../../components/applicationUi/diaryHeading/DiaryHeading'
import { useTranslation } from 'react-i18next'

export function Diary() {
  const { t } = useTranslation()
  return (
    <>
      <PageHeading>
        <DiaryHeading />
      </PageHeading>
      <PageBody>
        <nav className="my-1 flex gap-2">
          <Link
            className="rounded-md bg-slate-600 px-2.5 py-1.5 text-sm font-semibold text-white shadow-sm hover:bg-slate-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-slate-600"
            to="show-diary"
          >
            {t('diary.main-link')}
          </Link>
          <Link
            className="rounded-md bg-slate-600 px-2.5 py-1.5 text-sm font-semibold text-white shadow-sm hover:bg-slate-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-slate-600"
            to="add-food"
          >
            {t('diary.add-food-link')}
          </Link>
          {/* <Link
            className="inline-flex items-center gap-x-1.5 rounded-md bg-slate-600 px-2.5 py-1.5 text-sm font-semibold text-white shadow-sm hover:bg-slate-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-slate-600"
            to="add-meal"
          >
            {t('diary.add-meal-link')}
          </Link> */}
        </nav>

        <Outlet />
      </PageBody>
    </>
  )
}
