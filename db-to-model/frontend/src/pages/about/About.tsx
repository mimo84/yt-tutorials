import { useTranslation } from 'react-i18next'
import PageBody from '../../components/Layout/PageBody'
import PageHeading from '../../components/Layout/PageHeading'

function About() {
  const { t } = useTranslation()
  return (
    <>
      <PageHeading>{t('about.title')}</PageHeading>
      <PageBody>
        <p>{t('about.body')}</p>
      </PageBody>
    </>
  )
}

export default About
