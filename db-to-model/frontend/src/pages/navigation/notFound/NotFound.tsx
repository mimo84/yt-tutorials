import { useTranslation } from 'react-i18next'
import { Link } from 'react-router-dom'

export default function NotFound() {
  const { t } = useTranslation()
  return (
    <div className="not-found-container">
      <h1>{t('notfound.title')}</h1>
      <Link to="/" className="link-button">
        {t('notfound.return')}
      </Link>
    </div>
  )
}
