interface ButtonProps {
  children: React.ReactElement
}
const Button = ({ children }: ButtonProps) => {
  return (
    <button
      type="button"
      className="inline-flex items-center gap-x-1.5 rounded-md bg-slate-600 px-2.5 py-1.5 text-sm font-semibold text-white shadow-sm hover:bg-slate-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-slate-600"
    >
      {children}
    </button>
  )
}
export default Button
