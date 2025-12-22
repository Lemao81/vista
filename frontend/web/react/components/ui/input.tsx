export type InputProps = {
  label: string;
  placeholder?: string;
  isPassword?: boolean;
};

export default function Input({ label, placeholder, isPassword }: InputProps) {
  return (
    <div className="flex flex-col w-full">
      <label className="leading-10 font-semibold">{label}</label>
      <input
        className="rounded-lg bg-white p-1 px-2 shadow-md ring-1 ring-neutral-400 focus:outline-0"
        type={isPassword ? 'password' : 'text'}
        placeholder={placeholder}
      />
    </div>
  );
}
