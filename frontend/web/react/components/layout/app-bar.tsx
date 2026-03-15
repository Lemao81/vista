import AuthButtonPanel from '@/components/auth/auth-button-panel';
import AppBarLogo from '@/components/layout/app-bar-logo';
import ColorSchemeSelector from '@/components/layout/color-scheme-selector';
import TopNavigation from '@/components/layout/top-navigation';

export default function AppBar() {
  return (
    <header className="flex justify-between bg-surface1 p-1.5">
      <AppBarLogo />
      <TopNavigation />
      <div className="flex gap-8">
        <ColorSchemeSelector />
        <AuthButtonPanel />
      </div>
    </header>
  );
}
