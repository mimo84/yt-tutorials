import { useTranslation } from 'react-i18next'
import PageBody from '../../components/Layout/PageBody'
import PageHeading from '../../components/Layout/PageHeading'

function Home() {
  const { t } = useTranslation()
  return (
    <>
      <PageHeading>{t('home.title')}</PageHeading>
      <PageBody>
        <p>{t('home.body')}</p>
      </PageBody>
    </>
  )
}

export default Home
