import { ReactNode } from 'react'

interface PageBodyProps {
  children: ReactNode
}

export default function PageBody({ children }: PageBodyProps) {
  return (
    <main>
      <div className="mx-auto max-w-7xl sm:px-6 lg:px-8">{children}</div>
    </main>
  )
}
