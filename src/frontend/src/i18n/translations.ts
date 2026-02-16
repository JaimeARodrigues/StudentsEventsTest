export const translations = {
  'pt-BR': {
    // Login Page
    login: {
      title: 'Event Manager',
      subtitle: 'Sistema de Gerenciamento de Eventos de Estudantes',
      welcomeBack: 'Bem-vindo de volta',
      signInMessage: 'Entre na sua conta para continuar',
      emailLabel: 'Endereço de E-mail',
      emailPlaceholder: 'seu@email.com',
      passwordLabel: 'Senha',
      passwordPlaceholder: '••••••••',
      signInButton: 'Entrar',
      signingIn: 'Entrando...',
      loginError: 'Falha no login. Verifique suas credenciais.',
      demoAccess: 'Acesso Demo',
      demoMessage: 'Use qualquer email e senha para entrar',
      footer: 'Desenvolvido para fins educacionais',
    },
    // Dashboard
    dashboard: {
      title: 'Event Manager',
      subtitle: 'Sistema de Gerenciamento de Eventos de Estudantes',
      sync: 'Sincronizar',
      syncing: 'Sincronizando...',
      logout: 'Sair',
      syncSuccess: 'Dados sincronizados com sucesso!',
      syncError: 'Falha ao sincronizar dados',
      loadUsersError: 'Falha ao carregar usuários',
      loadEventsError: 'Falha ao carregar eventos do usuário',
    },
    // User List
    userList: {
      title: 'Estudantes',
      students: 'estudantes',
      student: 'estudante',
      searchPlaceholder: 'Buscar estudantes por nome ou email...',
      noUsersFound: 'Nenhum estudante encontrado',
      noUsersMessage: 'Tente ajustar sua busca ou sincronizar dados',
    },
    // Event Detail
    eventDetail: {
      title: 'Agenda de Eventos',
      viewingEventsFor: 'Visualizando eventos de',
      totalEvents: 'Total de Eventos',
      upcoming: 'Próximos',
      past: 'Passados',
      noEventsScheduled: 'Nenhum evento agendado',
      noEventsMessage: 'Este estudante não possui eventos no momento',
      selectStudent: 'Selecione um estudante para ver os eventos',
      selectStudentMessage: 'Escolha um estudante da lista para ver seus eventos agendados',
      upcomingBadge: 'Próximo',
      location: 'Localização',
      organizer: 'Organizador',
      onlineMeeting: 'Reunião Online',
      duration: 'Duração',
    },
    // Common
    common: {
      loading: 'Carregando...',
      error: 'Erro',
      success: 'Sucesso',
    },
  },
  'en': {
    // Login Page
    login: {
      title: 'Event Manager',
      subtitle: 'Student Events Management System',
      welcomeBack: 'Welcome back',
      signInMessage: 'Sign in to your account to continue',
      emailLabel: 'Email Address',
      emailPlaceholder: 'your@email.com',
      passwordLabel: 'Password',
      passwordPlaceholder: '••••••••',
      signInButton: 'Sign In',
      signingIn: 'Signing in...',
      loginError: 'Login failed. Please check your credentials.',
      demoAccess: 'Demo Access',
      demoMessage: 'Use any email and password to login',
      footer: 'Developed for educational purposes',
    },
    // Dashboard
    dashboard: {
      title: 'Event Manager',
      subtitle: 'Student Events Management System',
      sync: 'Sync',
      syncing: 'Syncing...',
      logout: 'Logout',
      syncSuccess: 'Data synchronized successfully!',
      syncError: 'Failed to synchronize data',
      loadUsersError: 'Failed to load users',
      loadEventsError: 'Failed to load user events',
    },
    // User List
    userList: {
      title: 'Students',
      students: 'students',
      student: 'student',
      searchPlaceholder: 'Search students by name or email...',
      noUsersFound: 'No students found',
      noUsersMessage: 'Try adjusting your search or sync data',
    },
    // Event Detail
    eventDetail: {
      title: 'Events Schedule',
      viewingEventsFor: 'Viewing events for',
      totalEvents: 'Total Events',
      upcoming: 'Upcoming',
      past: 'Past',
      noEventsScheduled: 'No events scheduled',
      noEventsMessage: 'This student has no events at the moment',
      selectStudent: 'Select a student to view events',
      selectStudentMessage: 'Choose a student from the list to see their scheduled events',
      upcomingBadge: 'Upcoming',
      location: 'Location',
      organizer: 'Organizer',
      onlineMeeting: 'Online Meeting',
      duration: 'Duration',
    },
    // Common
    common: {
      loading: 'Loading...',
      error: 'Error',
      success: 'Success',
    },
  },
} as const

export type Language = keyof typeof translations
