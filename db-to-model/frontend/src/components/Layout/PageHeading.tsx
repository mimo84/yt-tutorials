import { ReactNode } from 'react'

interface PageHeadingProps {
  children: ReactNode
}
export default function PageHeading({ children }: PageHeadingProps) {
  return (
    <>
      <header>
        <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
          <h1 className="text-3xl font-bold leading-tight tracking-tight text-gray-900">
            {children}
          </h1>
        </div>
      </header>
    </>
  )
}
