export default function SideNavigation() {
  return (
    <nav className="side-nav flex flex-col gap-1 bg-neutral-300 p-1 pt-4">
      <a className="inline-flex items-center justify-center h-12 bg-neutral-400" href="/1">
        Link 1
      </a>
      <a className="inline-flex items-center justify-center h-12 bg-neutral-400" href="/2">
        Link 2
      </a>
      <a className="inline-flex items-center justify-center h-12 bg-neutral-400" href="/3">
        Link 3
      </a>
    </nav>
  );
}
